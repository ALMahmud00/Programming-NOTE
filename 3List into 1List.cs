public async Task<GetInventoryReportLandingDTO> GetInventoryReportLandingPasignation(long accountId, long branchId, DateTime fromDate, DateTime toDate, string viewOrder, long pageNo, long pageSize)
        {
            var invList = _contextR.InventoryHeaders.Where(d => d.AccountId == accountId
                          && d.BranchId == branchId && d.TransactionDate.Date >= fromDate.Date && d.TransactionDate.Date <= toDate.Date && d.IsActive == true).ToList();
            var itemList = _contextR.Items.Where(i =>i.AccountId == accountId && i.BranchId == branchId 
                           && (i.ItemTypeId != 3 && i.ItemTypeId!=4)&& i.IsActive == true).ToList();

            var opening = (from a in _contextR.InventoryHeaders
                           join b in _contextR.InventoryLines on a.InventoryTransId equals b.InventoryTransId
                           join i in _contextR.Items on b.ItemId equals i.ItemId
                           where a.TransactionDate.Date < fromDate.Date
                           && a.AccountId == accountId && a.BranchId == branchId && a.IsActive == true && i.IsActive == true
                           select new { b.TransQuantity, i.ItemName, b.ItemId })
                           .GroupBy(s => new { s.ItemId, s.ItemName })
                           .Select(g => new
                           {
                               g.Key.ItemId,
                               g.Key.ItemName,
                               total = g.Sum(c => c.TransQuantity)
                           }).ToList();

            var InventoryPlus = (from a in invList
                                 join b in _contextR.InventoryLines on a.InventoryTransId equals b.InventoryTransId
                                 join i in _contextR.Items on b.ItemId equals i.ItemId
                                 where b.TransQuantity > 0
                                 select new { b.TransQuantity, i.ItemName, b.ItemId })
                .GroupBy(s => new { s.ItemId, s.ItemName })
                .Select(g => new
                {
                    g.Key.ItemId,
                    g.Key.ItemName,
                    total = g.Sum(c => c.TransQuantity)
                }).ToList();

            var InventoryMinus = (from a in invList
                                  join b in _contextR.InventoryLines on a.InventoryTransId equals b.InventoryTransId
                                  join i in _contextR.Items on b.ItemId equals i.ItemId
                                  where b.TransQuantity < 0
                                  select new { b.TransQuantity, i.ItemName, b.ItemId })
                .GroupBy(s => new { s.ItemId, s.ItemName })
                .Select(g => new
                {
                    g.Key.ItemId,
                    g.Key.ItemName,
                    total = g.Sum(c => c.TransQuantity)
                }).ToList();

            var invR = (from i in itemList
                        let open = (from a in opening where a.ItemId == i.ItemId select a.total).FirstOrDefault()
                        let plus = (from a in InventoryPlus where a.ItemId == i.ItemId select a.total).FirstOrDefault()
                        let minus = (from a in InventoryMinus where a.ItemId == i.ItemId select a.total).FirstOrDefault()
                        select new GetInventoryReportDTO()
                        {
                            ItemId = i.ItemId,
                            ItemCode = i.ItemCode,
                            ItemName = i.ItemName,
                            Uom = i.UomName,
                            ItemIn = plus,
                            ItemOut = minus * -1,
                            Opening = open,
                            Closing = open + plus + minus
                        }).ToList();

            var counts = invR.Count();
            var data = invR.AsQueryable();
            data = viewOrder.ToUpper() switch
            {
                "ASC" => data.OrderBy(o => o.ItemId),
                "DESC" => data.OrderByDescending(o => o.ItemId),
                _ => data
            };

            if (pageNo <= 0)
                pageNo = 1;
            var itemData = PagingList<GetInventoryReportDTO>.CreateAsync(data, pageNo, pageSize);

            long index = (int)(1 + ((pageNo - 1) * pageSize));
            foreach (var item in itemData)
            {
                item.Sl = index++;
            }

            var objInfo = new GetInventoryReportLandingDTO
            {
                Data = itemData,
                PageSize = pageSize,
                TotalCount = counts,
                CurrentPage = pageNo
            };
            return objInfo;
        }
