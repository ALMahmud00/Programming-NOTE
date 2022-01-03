public async Task<List<GetFiveYearsPlanLandingDTO>> GetFiveYearsPlanLanding(long accountId, long businessUnitId, long sbuId, long yearId)
        {

            var rowData = await Task.FromResult(from h in _contextR.TblStrategicParticulars
                                                join r in _contextR.TblStrategicParticularsRow on h.IntStrategicParticularsId equals r.IntStrategicParticularsId
                                                where h.IntAccountId == accountId && h.IntBusinessUnitId == businessUnitId && h.IntYearId == yearId
                                                && h.IsForSbu == true && h.IntSbuid == sbuId && h.IsActive == true
                                                select r);
            var yearData = await Task.FromResult(_contextR.TblYear.Select(x=>x));

            var data =  await Task.FromResult((from c in _contextR.TblStrategicParticulars
                                          where c.IntAccountId == accountId && c.IntBusinessUnitId == businessUnitId && c.IntYearId == yearId
                                          && c.IsForSbu == true && c.IntSbuid == sbuId && c.IsActive == true
                                          orderby c.IntStrategicParticularsId ascending
                                          select new GetFiveYearsPlanLandingDTO
                                          {
                                              Frequency = c.IntPmsfrequencyId,
                                              StrategicParticularsId = c.IntStrategicParticularsId,
                                              StrategicParticularsName = c.StrStrategicParticularsName,
                                              OwnerId = c.IntOwnerId,
                                              OwnerName = c.StrOwnerName,
                                              CategoryId = c.IntStrategicParticularsTypeId,
                                              CategoryName = c.StrStrategicParticularsTypeName,
                                              Timeline = rowData.Where(x=>x.IntStrategicParticularsId == c.IntStrategicParticularsId)
                                                                .OrderByDescending(x=>x.DteEndDate).Select(x=>x.DteEndDate).FirstOrDefault(),
                                          }).ToList());
            foreach(var item in data)
            {
                if (item.Frequency == 2) // monthly
                {
                    var gp = (rowData.Where(x => x.IntStrategicParticularsId == item.StrategicParticularsId)
                                    .Select(a => new
                                    {
                                        YearId = a.IntYearId,
                                        Year = yearData.Where(x => x.IntYearId == a.IntYearId).Select(x => x.StrYearName).FirstOrDefault(),
                                        MonthId = a.IntMonthId,
                                        Target = a.NumTarget
                                    })).OrderBy(x=>x.YearId).ToList().GroupBy(x => x.YearId);

                    var resultList = new List<YearQuartersDTO>();
                    foreach (var group in gp)
                    {
                        var resultObj = new YearQuartersDTO();

                        resultObj.Year = group.Select(x => x.Year).FirstOrDefault();
                        resultObj.YearId = group.Select(x => x.YearId).FirstOrDefault();
                        resultObj.Q1 = group.Where(x => x.MonthId == 13 || x.MonthId == 14 || x.MonthId == 15).Select(x => x.Target).Sum();
                        resultObj.Q2 = group.Where(x => x.MonthId == 16 || x.MonthId == 17 || x.MonthId == 18).Select(x => x.Target).Sum();
                        resultObj.Q3 = group.Where(x => x.MonthId == 19 || x.MonthId == 20 || x.MonthId == 21).Select(x => x.Target).Sum();
                        resultObj.Q4 = group.Where(x => x.MonthId == 22 || x.MonthId == 23 || x.MonthId == 24).Select(x => x.Target).Sum();

                        resultList.Add(resultObj);
                    }
                    item.FiscalYears = resultList.OrderBy(x=>x.YearId).ToList();
                }
                else if (item.Frequency == 3) //quarterly
                {
                    var gp = (rowData.Where(x => x.IntStrategicParticularsId == item.StrategicParticularsId)
                                      .Select(a => new
                                      {
                                          YearId = a.IntYearId,
                                          Year = yearData.Where(x => x.IntYearId == a.IntYearId).Select(x => x.StrYearName).FirstOrDefault(),
                                          QuarterId = a.IntQuarterId,
                                          Target = a.NumTarget
                                      })).OrderBy(x => x.YearId).ToList().GroupBy(x => x.YearId);

                    var resultList = new List<YearQuartersDTO>();
                    foreach (var group in gp)
                    {
                        var resultObj = new YearQuartersDTO();

                        resultObj.Year = group.Select(x => x.Year).FirstOrDefault();
                        resultObj.YearId = group.Select(x => x.YearId).FirstOrDefault();
                        resultObj.Q1 = group.Where(x => x.QuarterId == 5).Select(x => x.Target).FirstOrDefault();
                        resultObj.Q2 = group.Where(x => x.QuarterId == 6).Select(x => x.Target).FirstOrDefault();
                        resultObj.Q3 = group.Where(x => x.QuarterId == 7).Select(x => x.Target).FirstOrDefault();
                        resultObj.Q4 = group.Where(x => x.QuarterId == 8).Select(x => x.Target).FirstOrDefault();

                        resultList.Add(resultObj);
                    }
                    item.FiscalYears = resultList.OrderBy(x => x.YearId).ToList();
                }
                if (item.Frequency == 4) //yearly
                {
                    var gp = rowData.Where(x => x.IntStrategicParticularsId == item.StrategicParticularsId)
                                    .Select(a => new YearQuartersDTO()
                                    {
                                        YearId = a.IntYearId,
                                        Year = yearData.Where(x => x.IntYearId == a.IntYearId).Select(x => x.StrYearName).FirstOrDefault(),
                                        Q1 = 0,
                                        Q2 = 0,
                                        Q3 = 0,
                                        Q4 = a.NumTarget
                                    }).OrderBy(x => x.YearId).ToList();

                    item.FiscalYears = gp;
                }
            }

            return data;
        }
