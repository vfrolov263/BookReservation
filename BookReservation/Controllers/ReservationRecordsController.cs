using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationRecordsController : ControllerBase
    {
        private readonly ReservationContext _context;

        public ReservationRecordsController(ReservationContext context)
        {
            _context = context;

            if (!_context.ReservationRecords.Any())
                FillBD();
        }

        // Sample initializing BD
        private void FillBD()
        {
            for (int i = 0; i < 10; ++i)
                _context.ReservationRecords.Add(new ReservationRecord(i + 1, "available", "-"));

            _context.SaveChanges();
        }

        // POST: api/ReservationRecords/reserve
        // Reserve entity
        [HttpPost("reserve")]
        public async Task<ActionResult> PostReservationRecord(long id, string title, string author)
        {
            if (!ReservationRecordIsEmpty(id))
                return NotFound();

            ReservationRecord newRec = _context.ReservationRecords.Find(id);
            newRec.title = title;
            newRec.author = author;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/ReservationRecords/release
        // Release entity
        [HttpPost("release")]
        public async Task<ActionResult> PostReservationRecord(long id)
        {
            if (!ReservationRecordExist(id))
                return NotFound();
            else if (ReservationRecordIsEmpty(id))
                return Ok();

            ReservationRecord newRec = _context.ReservationRecords.Find(id);
            newRec.title = "available";
            newRec.author = "-";
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/ReservationRecords/reserved
        // Return reserved elements
        [HttpGet("reserved")]
        public async Task<ActionResult<IEnumerable<ReservationRecord>>> GetReservedRecords()
        {
            var reservationRecord = await _context.ReservationRecords.Where(e => e.title != "available").ToListAsync();
            return (reservationRecord == null) ? NotFound() : reservationRecord;
        }

        // GET: api/ReservationRecords/available
        // Return available elements
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ReservationRecord>>> GetEmptyRecords()
        {
            var reservationRecord = await _context.ReservationRecords.Where(e => e.title == "available").ToListAsync();
            return (reservationRecord == null) ? NotFound() : reservationRecord;
        }

        private bool ReservationRecordExist(long id)
        {
            return _context.ReservationRecords.Any(e => e.id == id);
        }

        private bool ReservationRecordIsEmpty(long id)
        {
            return (ReservationRecordExist(id) && _context.ReservationRecords.Find(id).title == "available");
        }
    }
}
