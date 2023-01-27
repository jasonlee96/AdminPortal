# Structure on usual context

var context = outerContext ?? await _factory.CreateDbContextAsync();
try
{

}
catch
{
    throw;
}
finally
{
    if(outerContext == null) await context.DisposeAsync();
}


#Transactions