using Bigprofits.Data;
using Bigprofits.Models;

namespace Bigprofits.Repository
{
    public class HomeRepository(ContextClass context)
    {
        private readonly ContextClass context = context;

        public string LeftLink()
        {
            return "https://Bigprofits.biz/Home/Registration?spoId=@ViewBag.memberid&RefPos=LEFT";
        }

        public async Task<List<ListCountry>> CountryList()
        {
            return await context.ListCountries.Select(x => new ListCountry()
            {
                Cid = x.Cid,
                Cname = x.Cname

            }).ToListAsync();
        }

        public async Task<List<ListState>> StateList()
        {
            return await context.ListStates.Select(x => new ListState()
            {
                Sid = x.Sid,
                Sname = x.Sname

            }).ToListAsync();
        }

        public async Task<List<PinDetail>> PinDetail()
        {
            return await context.PinDetails.Select(x => new PinDetail()
            {
                PinType = x.PinType,
                PinAmout = x.PinAmout,
                PinDetail1 = x.PinDetail1
                
            }).ToListAsync();
        }

        public async Task<List<ProductCat>> ProductCat()
        {
            return await context.ProductCats.Select(x => new ProductCat()
            {
                CatName = x.CatName,
                Id = x.Id

            }).ToListAsync();
        }

        public async Task<List<ProductSubCat>> ProductSubCat()
        {
            return await context.ProductSubCats.Select(x => new ProductSubCat()
            {
                SubCatName = x.SubCatName,
                Id = x.Id
            }).ToListAsync();
        }

        public string GetPaginationBtn(string url, int pageSize, int recordCount, int currentPage)
        {
            string res = "";
            if (currentPage == 0) currentPage = 1;

            int pagerSpan = 5;
            int startIndex, endIndex;
            double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(pageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
            _ = pageCount > pagerSpan ? pagerSpan : pageCount;
            if (currentPage > pagerSpan % 2)
            {
                if (currentPage == 2) endIndex = 5;
                else endIndex = currentPage + 2;
            }
            else endIndex = (pagerSpan - currentPage) + 1;
            if (endIndex - (pagerSpan - 1) > startIndex) startIndex = endIndex - (pagerSpan - 1);
            if (endIndex > pageCount)
            {
                endIndex = pageCount;
                startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
            }

            if (currentPage > 1) res += $"<li class='page-item'><a class='page-link' href='{url}1'>First</a></li>";
            if (currentPage > 1) res += $"<li class='page-item'><a class='page-link' href='{url}{(currentPage - 1)}'><<</a></li>";

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (currentPage == i) res += $"<li class='page-item active'><a class='page-link'>{i}</a></li>";
                else res += $"<li class='page-item'><a class='page-link' href='{url}{i}'>{i}</a></li>";
            }

            if (currentPage < pageCount) res += $"<li class='page-item'><a class='page-link' href='{url}{(currentPage + 1)}'>>></a></li>";
            if (currentPage != pageCount) res += $"<li class='page-item'><a class='page-link' href='{url}{pageCount}'>{pageCount}</a></li>";

            //int pageCount = (rowCount / pageSize);

            //if (pageCount > 1)
            //{
            //    if ((pageNo - 1) > 0)
            //    {
            //        res += $"<li class='page-item'><a class='page-link' href='{url}{(pageNo - 1)}'><<</a></li>";
            //        res += $"<li class='page-item'><a class='page-link' href='{url}'>First</a></li>";
            //    }
            //    for (int i = 1; i <= pageCount; i++)
            //    {
            //        if (pageNo == i) res += $"<li class='page-item active'><a class='page-link' href='{url}{i}'>{i}</a></li>";
            //        else res += $"<li class='page-item'><a class='page-link' href='{url}{i}'>{i}</a></li>";
            //    }
            //    if (pageCount > (pageNo + 1))
            //    {
            //        res += $"<li class='page-item'><a class='page-link' href='{url}{(pageNo + 1)}'>>></a></li>";
            //        res += $"<li class='page-item'><a class='page-link' href='{url}{pageCount}'>{pageCount}</a></li>";
            //    }
            //}
            //else
            //{
            //    res += "<li class='page-item disabled'><a class='page-link'><<</a></li><li class='page-item active'><a class='page-link'>1</a></li><li class='page-item disabled'><a class='page-link'>>></a></li>";
            //}
            return res;
        }

    }
}
