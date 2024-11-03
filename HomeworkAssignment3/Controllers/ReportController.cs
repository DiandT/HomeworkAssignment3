using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HomeworkAssignment3.Models; // Adjust this namespace as per your project
using System.Data.Entity;

public class ReportController : Controller
{
    private readonly LibraryContext _context;

    public ReportController()
    {
        _context = new LibraryContext();
    }

    // GET: Report - Shows the report form
    public ActionResult Index()
    {
        return View();
    }

    // POST: Report/StudentBorrowingRanking - Generates the Student Borrowing Ranking Report
    [HttpPost]
    public async Task<ActionResult> StudentBorrowingRanking(DateTime? startDate, DateTime? endDate)
    {
        // Set default values if dates are not provided
        DateTime defaultStartDate = startDate ?? DateTime.MinValue;
        DateTime defaultEndDate = endDate ?? DateTime.MaxValue;

        var ranking = await _context.Students
            .Select(s => new StudentBorrowingRankingViewModel
            {
                StudentName = s.Name,
                BorrowCount = s.Borrows.Count(b =>
                    DbFunctions.TruncateTime(b.TakenDate) >= DbFunctions.TruncateTime(defaultStartDate) &&
                    DbFunctions.TruncateTime(b.TakenDate) <= DbFunctions.TruncateTime(defaultEndDate))
            })
            .Where(s => s.BorrowCount > 0) // Exclude students with a borrow count of zero
            .OrderByDescending(s => s.BorrowCount)
            .ToListAsync();

        // Set ViewBag properties to retain selected dates in the view
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;

        return View("~/Views/Home/Reports.cshtml", ranking);
    }

    // POST: Report/ExportStudentBorrowingRanking - Exports the Student Borrowing Ranking report
    [HttpPost]
    public async Task<ActionResult> ExportStudentBorrowingRanking(DateTime? startDate, DateTime? endDate, string fileName, string fileType, string description)
    {
        System.Diagnostics.Debug.WriteLine($"Export StartDate: {startDate}, EndDate: {endDate}");

        // Check if startDate and endDate are provided
        if (!startDate.HasValue || !endDate.HasValue)
        {
            return Content("Start date and end date must be provided.");
        }

        // Generate the ranking data for the specified date range
        var ranking = await _context.Students
            .Select(s => new StudentBorrowingRankingViewModel
            {
                StudentName = s.Name,
                BorrowCount = s.Borrows.Count(b =>
                    DbFunctions.TruncateTime(b.TakenDate) >= DbFunctions.TruncateTime(startDate.Value) &&
                    DbFunctions.TruncateTime(b.TakenDate) <= DbFunctions.TruncateTime(endDate.Value))
            })
            .Where(s => s.BorrowCount > 0) // Exclude students with a borrow count of zero
            .OrderByDescending(s => s.BorrowCount)
            .ToListAsync();

        System.Diagnostics.Debug.WriteLine($"Ranking Count: {ranking.Count()}");

        // Check if ranking is empty
        if (!ranking.Any())
        {
            return Content("No data found for the given date range.");
        }

        // Prepare the file path and ensure the directory exists
        string directoryPath = Server.MapPath("~/Reports");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // Create directory if it does not exist
        }

        string filePath = Path.Combine(directoryPath, $"{fileName}.{fileType}");

        // Write data to the file
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Optionally include the description at the top of the file
                if (!string.IsNullOrEmpty(description))
                {
                    writer.WriteLine("Description:");
                    writer.WriteLine(description);
                    writer.WriteLine(); // Blank line for separation
                }

                // Write the header line
                writer.WriteLine("Student Name,Borrow Count");

                // Write each entry
                foreach (var entry in ranking)
                {
                    writer.WriteLine($"{entry.StudentName},{entry.BorrowCount}");
                }
            }

            // Return the file for download
            return File(filePath, "application/octet-stream", $"{fileName}.{fileType}");
        }
        catch (UnauthorizedAccessException uex)
        {
            System.Diagnostics.Debug.WriteLine($"Access error: {uex.Message}");
            return Content("Error: Access denied when writing to the file.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error writing to file: {ex.Message}");
            return Content($"Error writing to file: {ex.Message}");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}
