using MainSite.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainSite.Extensions
{
    public static class PageExtensions
    {
        public static void Pager(this PagerViewModel model)
        {
            model.ViewPageList = new List<PagerElementModel>();
            if (model.TotalRecords == 0)
                return;


            if (model.ShowPagerItems && (model.TotalPages > 1))
            {
                if (model.ShowIndividualPages)
                {
                    //individual pages
                    var firstIndividualPageIndex = model.GetFirstIndividualPageIndex();
                    var lastIndividualPageIndex = model.GetLastIndividualPageIndex();
                    for (var i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            model.ViewPageList.Add(
                                new PagerElementModel()
                                {
                                    Index = model.PageIndex + 1,
                                    IsActive = true
                                });
                        }
                        else
                        {
                            model.ViewPageList.Add(new PagerElementModel()
                            {
                                Index = (i + 1),
                                IsActive = false
                            });
                        }
                    }
                }
            }
        }
    }
}
