using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BillingSystem.Models;

namespace BillingSystem.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly BillingDBEntities db = new BillingDBEntities();

        // GET: Invoices
        public async Task<ActionResult> Index()
        {
            var invoices = await db.Invoices
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return View(invoices);
        }

        // GET: Invoices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var invoice = await db.Invoices
                .Include("InvoiceDetails")
                .FirstOrDefaultAsync(i => i.InvoiceId == id.Value);

            if (invoice == null)
                return HttpNotFound();

            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View(new Invoice
            {
                InvoiceDate = DateTime.Now,
                IsActive = true
            });
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "InvoiceNumber,InvoiceDate,CustomerName,TotalAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.IsActive = true;
                // TotalAmount comes from the form now; do not overwrite it here.

                db.Invoices.Add(invoice);
                await db.SaveChangesAsync();

                return RedirectToAction("Details", new { id = invoice.InvoiceId });
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var invoice = await db.Invoices.FindAsync(id.Value);
            if (invoice == null)
                return HttpNotFound();

            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "InvoiceId,InvoiceNumber,InvoiceDate,CustomerName")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                var dbInvoice = await db.Invoices.FindAsync(invoice.InvoiceId);
                if (dbInvoice == null)
                    return HttpNotFound();

                dbInvoice.InvoiceDate = invoice.InvoiceDate;
                dbInvoice.CustomerName = invoice.CustomerName;

                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = invoice.InvoiceId });
            }

            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var invoice = await db.Invoices.FindAsync(id.Value);
            if (invoice == null)
                return HttpNotFound();

            return View(invoice);
        }

        // POST: Invoices/Delete/5 (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var invoice = await db.Invoices.FindAsync(id);

            if (invoice != null && invoice.IsActive)
            {
                invoice.IsActive = false;
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}