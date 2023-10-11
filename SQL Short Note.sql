--===== primary keys affected by update 
declare @insertedIds table (id int)

update your_table
set table_field = 'field_value'
output inserted.your_table_primary_key into @insertedIds
where table_field = 'field_value'

select id from @insertedIds


--===== BASE-64 Encode-Decode
DECLARE @MyString varchar(250) = 'akijGlobe@6598'
DECLARE @Encoded varchar(250) 
DECLARE @Decoded varchar(250)  
 
set @Encoded = (SELECT CAST(@MyString as varbinary(max)) FOR XML PATH(''), BINARY BASE64)    
set @Decoded = (SELECT CAST( CAST(@Encoded as XML ).value('.','varbinary(max)') AS varchar(max))) 

select @Encoded, @Decoded


--===== Remove Duplicate
  WITH cte AS (
    SELECT 
        strThanaName, isactive,
        ROW_NUMBER() OVER (
            PARTITION BY 
                strThanaName 
            ORDER BY 
               strThanaName 
        ) row_num
     FROM 
        iBOSDDD.dco.TblThana
)
select * FROM cte
WHERE row_num > 1
order by row_num;



