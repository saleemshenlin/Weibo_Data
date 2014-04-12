using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using weibo_data.Models;
using weibo_data.Dals;

namespace weibo_data.Controllers
{
    /*
    若要为此控制器添加路由，请将这些语句合并到 WebApiConfig 类的 Register 方法中。请注意 OData URL 区分大小写。

    using System.Web.Http.OData.Builder;
    using weibo_data.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<WeiboFromBigData>("Weibo");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class WeiboController : ODataController
    {
        private WeiboBigDataContext db = new WeiboBigDataContext();

        // GET odata/Weibo
        [Queryable]
        public IQueryable<WeiboFromBigData> GetWeibo()
        {
            return db.WeiboFromBigDatas;
        }

        // GET odata/Weibo(5)
        [Queryable]
        public SingleResult<WeiboFromBigData> GetWeiboFromBigData([FromODataUri] int key)
        {
            return SingleResult.Create(db.WeiboFromBigDatas.Where(weibofrombigdata => weibofrombigdata.Id == key));
        }

        // PUT odata/Weibo(5)
        public IHttpActionResult Put([FromODataUri] int key, WeiboFromBigData weibofrombigdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != weibofrombigdata.Id)
            {
                return BadRequest();
            }

            db.Entry(weibofrombigdata).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeiboFromBigDataExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(weibofrombigdata);
        }

        // POST odata/Weibo
        public IHttpActionResult Post(WeiboFromBigData weibofrombigdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WeiboFromBigDatas.Add(weibofrombigdata);
            db.SaveChanges();

            return Created(weibofrombigdata);
        }

        // PATCH odata/Weibo(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<WeiboFromBigData> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WeiboFromBigData weibofrombigdata = db.WeiboFromBigDatas.Find(key);
            if (weibofrombigdata == null)
            {
                return NotFound();
            }

            patch.Patch(weibofrombigdata);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeiboFromBigDataExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(weibofrombigdata);
        }

        // DELETE odata/Weibo(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            WeiboFromBigData weibofrombigdata = db.WeiboFromBigDatas.Find(key);
            if (weibofrombigdata == null)
            {
                return NotFound();
            }

            db.WeiboFromBigDatas.Remove(weibofrombigdata);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeiboFromBigDataExists(int key)
        {
            return db.WeiboFromBigDatas.Count(e => e.Id == key) > 0;
        }
    }
}
