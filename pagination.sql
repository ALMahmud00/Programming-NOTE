
declare @pageNo bigint = 3, @pageSize bigint = 20

SELECT intEmployeeBasicInfoId,
@pageNo AS pageNo, 
@pageSize as pageSize, 
ROW_NUMBER() OVER(ORDER BY intEmployeeBasicInfoId ASC) AS serial, 
COUNT(*) OVER () AS totalCount
FROM saas.empEmployeeBasicInfo  
ORDER BY intEmployeeBasicInfoId ASC				
OFFSET (@pageNo-1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY
