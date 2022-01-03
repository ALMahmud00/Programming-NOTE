--===== primary keys affected by update 
declare @insertedIds table (id int)

update your_table
set table_field = 'field_value'
output inserted.your_table_primary_key into @insertedIds
where table_field = 'field_value'

select id from @insertedIds
