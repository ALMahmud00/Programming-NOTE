---Function to find Date Duration
ALTER FUNCTION [timesheet].[funDateDurationYearMonthDay] 
(
	@dteStartDate date, @dteEndDate date
)
RETURNS varchar(200)
AS
BEGIN

	Declare @strAgeCalculator varchar(200)

	Set @dteStartDate = dateadd(day,-1,@dteStartDate)

	Select @strAgeCalculator = case when @dteStartDate >= @dteEndDate Then 'N/A'
		Else case when DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ), @dteStartDate ) > @dteEndDate 
		Then case when ((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) / 12) > 1 
				then cast((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) / 12 as varchar(20)) + ' Years ' 
				else 
				case when ((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) / 12) = 1 
					then cast((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) / 12 as varchar(20)) + ' Year ' 
					else '' 
					end 
				end +

			case when ((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) % 12) > 1 
				then cast((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) % 12 as varchar(20)) + ' Months ' 
				else 
				case when ((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) % 12) = 1 
					then cast((datediff ( month, @dteStartDate, @dteEndDate ) - 1 ) % 12 as varchar(20)) + ' Month ' 
					else '' 
					end 
				end + 

			case when (datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ) - 1, @dteStartDate ),@dteEndDate)) > 1 
				then cast(datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ) - 1, @dteStartDate ),@dteEndDate) as varchar(20)) + ' Days' 
				else 
				case when (datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ) - 1, @dteStartDate ),@dteEndDate)) = 1 
					then cast(datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ) - 1, @dteStartDate ),@dteEndDate) as varchar(20)) + ' Day' 
					else '' 
					end 
				end

		Else case when (datediff ( month, @dteStartDate, @dteEndDate ) / 12) > 1 
			then cast(datediff ( month, @dteStartDate, @dteEndDate ) / 12 as varchar(20)) + ' Years ' 
			else 
			case when (datediff ( month, @dteStartDate, @dteEndDate ) / 12) = 1 
				then cast(datediff ( month, @dteStartDate, @dteEndDate ) / 12 as varchar(20)) + ' Year ' 
				else '' 
				end 
			end +

		case when (datediff ( month, @dteStartDate, @dteEndDate ) % 12) > 1 
			then cast(datediff ( month, @dteStartDate, @dteEndDate ) % 12 as varchar(20)) + ' Months ' 
			else 
			case when (datediff ( month, @dteStartDate, @dteEndDate ) % 12) = 1 
				then cast(datediff ( month, @dteStartDate, @dteEndDate ) % 12 as varchar(20)) + ' Month ' 
				else '' 
				end 
			end + 
			
		case when (datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ), @dteStartDate ),@dteEndDate)) > 1 
			then cast(datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ), @dteStartDate ),@dteEndDate) as varchar(20)) + ' Days' 
			else 
			case when (datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ), @dteStartDate ),@dteEndDate)) = 1 
				then cast(datediff (day,DateAdd ( month, datediff ( month, @dteStartDate, @dteEndDate ), @dteStartDate ),@dteEndDate) as varchar(20)) + ' Day' 
				else '' 
				end 
			end 
		end 
	end
	
	Return @strAgeCalculator

END
