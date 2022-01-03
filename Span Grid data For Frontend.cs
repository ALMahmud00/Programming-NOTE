public async Task<List<GetTotalSalesSummeryDTO>> TotalSalesSummery(long accountId, long branchId, DateTime fromDate, DateTime toDate)
        {

            var result = await Task.FromResult((from so in _contextR.SalesOrders
                                                join sl in _contextR.SubLedgerHeaders on so.SalesOrderId equals sl.TransactionId
                                                where sl.TransactionTypeId == 2
                                                join sr in _contextR.SubLedgerRows on sl.SubLedgerHeaderId equals sr.SubLedgerHeaderId
                                                where sl.TransactionDate.Date >= fromDate.Date && sl.TransactionDate <= toDate.Date
                                                && sl.AccountId == accountId && sl.BranchId == branchId && sr.ChartOfAccId == 1
                                                select new
                                                {
                                                    so.SalesForceId,
                                                    so.SalesForceName,
                                                    so.CustomerId,
                                                    so.CustomerName,
                                                    sr.Amount
                                                }).GroupBy(x => new
                                                {
                                                    x.SalesForceId,
                                                    x.SalesForceName,
                                                    x.CustomerId,
                                                    x.CustomerName,
                                                }).Select(x=> new GetTotalSalesSummeryDTO
                                                {
                                                   SalesforceId = x.Key.SalesForceId,
                                                   SalesforceName = x.Key.SalesForceId == 0 ? "Anonymous" : x.Key.SalesForceName,
                                                   CustomerId = x.Key.CustomerId,
                                                   CustomerName = x.Key.CustomerName,
                                                   Amount = x.Sum(a=>a.Amount)
                                                }).OrderBy(a=>a.SalesforceId).ThenBy(a=>a.CustomerId).ToList());

            long flagSalesforceId = -1;
            long index = 0;
            decimal spanTotalAmount = 0;
            result.ForEach(itm => 
            {
                if (flagSalesforceId != itm.SalesforceId)
                {
                    index++;
                    spanTotalAmount = result.Where(a => a.SalesforceId == itm.SalesforceId).Sum(a => a.Amount);
                    itm.SpanSerial = index;
                    itm.IsNewSpan = true;
                    itm.TotalAmount = spanTotalAmount;
                    flagSalesforceId = itm.SalesforceId;
                }
                else
                {
                    itm.SpanSerial = index;
                    itm.IsNewSpan = false;
                    itm.TotalAmount = spanTotalAmount;
                }
            });

            return result;
        }
