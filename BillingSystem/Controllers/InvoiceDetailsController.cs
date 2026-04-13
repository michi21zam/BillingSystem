using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BillingSystem.Models;

namespace BillingSystem.Controllers
{
    public class InvoiceDetailsController : Controller
    {
        private readonly BillingDBEntities db = new BillingDBEntities();

        // GET: InvoiceDetails/Create?invoiceId=5
        public ActionResult Create(int? invoiceId)
        {
            if (!invoiceId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(new InvoiceDetail
            {
                InvoiceId = invoiceId.Value,
                IsActive = true
            });
        }

        // POST: InvoiceDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "InvoiceId,ProductName,Quantity,UnitPrice")] InvoiceDetail detail)
        {
            if (ModelState.IsValid)
            {
                detail.IsActive = true;
                db.InvoiceDetails.Add(detail);
                await db.SaveChangesAsync();

                return RedirectToAction("Details", "Invoices",
                    new { id = detail.InvoiceId });
            }

            return View(detail);
        }

        // GET: InvoiceDetails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var detail = await db.InvoiceDetails.FindAsync(id.Value);
            if (detail == null || !detail.IsActive)
                return HttpNotFound();

            return View(detail);
        }

        // POST: InvoiceDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "InvoiceDetailId,InvoiceId,ProductName,Quantity,UnitPrice")] InvoiceDetail detail)
        {
            if (ModelState.IsValid)
            {
                var dbDetail = await db.InvoiceDetails.FindAsync(detail.InvoiceDetailId);
                if (dbDetail == null)
                    return HttpNotFound();

                dbDetail.ProductName = detail.ProductName;
                dbDetail.Quantity = detail.Quantity;
                dbDetail.UnitPrice = detail.UnitPrice;

                await db.SaveChangesAsync();

                return RedirectToAction("Details", "Invoices",
                    new { id = detail.InvoiceId });
            }

            return View(detail);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var detail = await db.InvoiceDetails.FindAsync(id.Value);
            if (detail == null || !detail.IsActive)
                return HttpNotFound();

            return View(detail);
        }

        // POST: InvoiceDetails/Delete/5 (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var detail = await db.InvoiceDetails.FindAsync(id);

            if (detail != null && detail.IsActive)
            {
                detail.IsActive = false;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Invoices", new { id = detail?.InvoiceId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
