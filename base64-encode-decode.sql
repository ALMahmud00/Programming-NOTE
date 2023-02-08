--===== BASE-64 Encode-Decode
DECLARE @MyString varchar(250) = 'akijGlobe@6598'
DECLARE @Encoded varchar(250) 
DECLARE @Decoded varchar(250)  
 
set @Encoded = (SELECT CAST(cast(@MyString as varchar(500)) as varbinary(max)) FOR XML PATH(''), BINARY BASE64)    
set @Decoded = (SELECT CAST( CAST(cast(@Encoded as varchar(500)) as XML ).value('.','varbinary(max)') AS varchar(max))) 

select @Encoded, @Decoded
