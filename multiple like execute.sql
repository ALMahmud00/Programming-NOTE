DECLARE @tblTemp TABLE(strPrefix NVARCHAR(100))
INSERT INTO @tblTemp VALUES('AC%'),('AY%'),('AN%'),('AZ%'),('IR%')

SELECT * 
from @tblTemp tm
join iBOSDDD.hcm.tblEmployeeBasicInfo em on em.strEmployeeFirstName like '%'+tm.strPrefix+'%'
