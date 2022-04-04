/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) Min([Id]) As Id
      ,[ProductFromSiteId]
      ,Min([ParseDate]) As ParseDate
      ,[FullPrice]
      ,[DiscountPrice]
      ,[DiscountPercent]
      ,[CurrencyCode]
      ,[IsOutOfStock]
      ,[ParseError]
      ,[CurrencyId]
Into #Temp_Data
  FROM [aspnet-PriceParser].[dbo].[ProductPricesHistory]
  Group by
  [ProductFromSiteId]
      
      ,[FullPrice]
      ,[DiscountPrice]
      ,[DiscountPercent]
      ,[CurrencyCode]
      ,[IsOutOfStock]
      ,[ParseError]
      ,[CurrencyId]


Delete From [aspnet-PriceParser].[dbo].[ProductPricesHistory]

INSERT INTO [aspnet-PriceParser].[dbo].[ProductPricesHistory]
SELECT * FROM #Temp_Data

Drop Table #Temp_Data

