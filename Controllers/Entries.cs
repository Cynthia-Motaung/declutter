using declutter.Data;
using declutter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;


namespace declutter.Controllers
{
    [Authorize]
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor to inject the ApplicationDbContext and UserManager.
        public EntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays a list of entries belonging to the currently logged-in user.
        public async Task<IActionResult> Index()
        {
            // Get the current user's ID.
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                ShowNotification("You must be logged in to view entries", "Error", NotificationType.Error);
                return Unauthorized();
            }

            var entries = await _context.Entries
                .Include(e => e.Tags)
                .Where(e => e.AuthorId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            // Get all available tags for the filter (only tags used by this user)
            var allTags = await _context.Tags
                .Where(t => t.Entries.Any(e => e.AuthorId == userId))
                .Distinct()
                .ToListAsync();

            ViewData["AllTags"] = allTags;

            return View(entries);
        }

    
        // Displays the details of a specific entry, ensuring it belongs to the current user.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ShowNotification("Entry not found", "Error", NotificationType.Error);
                return NotFound();
            }

            // Get the current user's ID.
            var userId = GetCurrentUserId();

            // Find the entry by ID and ensure it belongs to the current user.
            var entry = await _context.Entries
                                      .Include(e => e.Tags)
                                      .FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);

            if (entry == null)
            {
                ShowNotification("Entry not found or you don't have permission to view it", "Error", NotificationType.Error);
                return NotFound();
            }

            return View(entry);
        }

        public async Task<IActionResult> Create()
        {
            // Get the current user's ID.
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                ShowNotification("You must be logged in to create an entry", "Error", NotificationType.Error);
                return Unauthorized();
            }

            // Pass available tags for the current user
            ViewBag.AvailableTags = await _context.Tags
                .Where(t => t.Entries.Any(e => e.AuthorId == userId))
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Tags")] Entry entry, List<int> selectedTagIds)
        {
            // Get the current user's ID.
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                // If user ID is not found, it means the user is not authenticated properly.
                ShowNotification("You must be logged in to create an entry", "Error", NotificationType.Error);
                return Unauthorized();
            }

            // Manually set UserId and CreatedAt as they are not bindable from the form.
            entry.AuthorId = userId;
            entry.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Add selected tags to the entry
                if (selectedTagIds != null && selectedTagIds.Any())
                {
                    var selectedTags = await _context.Tags
                                                     .Where(t => selectedTagIds.Contains(t.Id))
                                                     .ToListAsync();
                    entry.Tags = selectedTags;
                }

                _context.Add(entry);
                await _context.SaveChangesAsync();
                ShowNotification("Entry created successfully", "Success", NotificationType.Success);
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, re-display the form with errors and available tags.
            ShowNotification("Please fix the validation errors", "Validation Error", NotificationType.Warning);
            ViewBag.AvailableTags = await _context.Tags
                .Where(t => t.Entries.Any(e => e.AuthorId == userId))
                .ToListAsync();
            return View(entry);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ShowNotification("Entry not found", "Error", NotificationType.Error);
                return NotFound();
            }

            // Get the current user's ID.
            var userId = GetCurrentUserId();

            // Find the entry by ID and ensure it belongs to the current user.
            // Include Tags to display current tags in the edit form.
            var entry = await _context.Entries
                                      .Include(e => e.Tags)
                                      .FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);

            if (entry == null)
            {
                ShowNotification("Entry not found or you don't have permission to edit it", "Error", NotificationType.Error);
                return NotFound();
            }

            // Pass available tags and current entry's selected tag IDs to the view
            ViewBag.AvailableTags = await _context.Tags
                .Where(t => t.Entries.Any(e => e.AuthorId == userId))
                .ToListAsync();
            ViewBag.SelectedTagIds = entry.Tags.Select(t => t.Id).ToList();

            return View(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Entry entry, List<int> selectedTagIds)
        {
            if (id != entry.Id)
            {
                ShowNotification("Entry not found", "Error", NotificationType.Error);
                return NotFound();
            }

            // Get the current user's ID.
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                ShowNotification("You must be logged in to edit an entry", "Error", NotificationType.Error);
                return Unauthorized();
            }

            // Retrieve the existing entry from the database, including its current tags.
            var entryToUpdate = await _context.Entries
                                              .Include(e => e.Tags)
                                              .FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);

            if (entryToUpdate == null)
            {
                // If the entry doesn't exist or doesn't belong to the user, return NotFound.
                ShowNotification("Entry not found or you don't have permission to edit it", "Error", NotificationType.Error);
                return NotFound();
            }

            // Update only the allowed properties.
            if (await TryUpdateModelAsync<Entry>(entryToUpdate,
                "", // Prefix for form fields (empty string means no prefix)
                e => e.Title, e => e.Content))
            {
                //entryToUpdate.UpdatedAt = DateTime.Now;

                // Update tags
                entryToUpdate.Tags.Clear(); // Clear existing tags
                if (selectedTagIds != null && selectedTagIds.Any())
                {
                    var newTags = await _context.Tags
                                                .Where(t => selectedTagIds.Contains(t.Id))
                                                .ToListAsync();
                    foreach (var tag in newTags)
                    {
                        entryToUpdate.Tags.Add(tag);
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    ShowNotification("Entry updated successfully", "Success", NotificationType.Success);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntryExists(entry.Id))
                    {
                        ShowNotification("Entry not found", "Error", NotificationType.Error);
                        return NotFound();
                    }
                    else
                    {
                        ShowNotification("An error occurred while updating the entry", "Error", NotificationType.Error);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If TryUpdateModelAsync fails, re-display the form with errors and available tags.
            ShowNotification("Please fix the validation errors", "Validation Error", NotificationType.Warning);
            ViewBag.AvailableTags = await _context.Tags
                .Where(t => t.Entries.Any(e => e.AuthorId == userId))
                .ToListAsync();
            ViewBag.SelectedTagIds = entryToUpdate.Tags.Select(t => t.Id).ToList();
            return View(entry);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ShowNotification("Entry not found", "Error", NotificationType.Error);
                return NotFound();
            }

            // Get the current user's ID.
            var userId = GetCurrentUserId();

            // Find the entry by ID and ensure it belongs to the current user.
            var entry = await _context.Entries
                                      .Include(e => e.Tags)
                                      .FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);

            if (entry == null)
            {
                ShowNotification("Entry not found or you don't have permission to delete it", "Error", NotificationType.Error);
                return NotFound();
            }

            return View(entry);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get the current user's ID.
            var userId = GetCurrentUserId();

            // Find the entry by ID and ensure it belongs to the current user.
            var entry = await _context.Entries.FirstOrDefaultAsync(e => e.Id == id && e.AuthorId == userId);

            if (entry != null)
            {
                _context.Entries.Remove(entry);
                await _context.SaveChangesAsync();
                ShowNotification("Entry deleted successfully", "Success", NotificationType.Success);
            }
            else
            {
                ShowNotification("Entry not found or you don't have permission to delete it", "Error", NotificationType.Error);
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an entry exists.
        private bool EntryExists(int id)
        {
            return _context.Entries.Any(e => e.Id == id);
        }

        // Helper method to get current user ID
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Helper method to show Toastr notifications
        private void ShowNotification(string message, string title, NotificationType type)
        {
            var notification = new
            {
                message,
                title,
                type = type.ToString().ToLower()
            };

            TempData["Notification"] = System.Text.Json.JsonSerializer.Serialize(notification);
        }

        // Notification type enum
        public enum NotificationType
        {
            Success,
            Error,
            Warning,
            Info
        }
    }
}