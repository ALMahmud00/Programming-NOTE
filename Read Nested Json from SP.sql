Read nested json from store procedure

declare @strSubjects NVARCHAR(MAX), @intClass BIGINT
declare @xmlTable NVARCHAR(MAX) = 
'[
  {
    "intClass": 10,
    "strSubList": [
      {
        "strSubject": "bangla"
      },
      {
        "strSubject": "english"
      }
    ]
  }
]'

select @intClass = intClass, @strSubjects = strSubList
from openjson(@xmlTable) with(intClass bigint, strSubList nvarchar(max) as json)

select @intClass
select strSubject
from openjson(@strSubjects) with(strSubject nvarchar(max))
