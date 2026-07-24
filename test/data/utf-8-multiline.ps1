#Requires -Modules SqlServer

function Get-Product([int]$id)
{
	Invoke-SqlCmd @"
select *
  from Products.Product
 where product_id = $id;
"@
}

Get-Product 1
