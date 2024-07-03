USE [MilkStore]
GO

INSERT INTO [dbo].[Accounts]
           ([RoleId]
           ,[UserName]
           ,[Phone]
           ,[Address]
           ,[Dob]
           ,[Email]
           ,[Password]
           ,[Status])
     VALUES
           (2
           ,'User1'
           ,0909090909
           ,'Address1'
           ,'2002-07-02 09:14:00.5280000'
           ,'user01@gmail.com'
           ,'4dff4ea340f0a823f15d3f4f01ab62eae0e5da579ccb851f8db9dfe84c58b2b37b89903a740e1ee172da793a6e79d560e5f7f9bd058a12a280433ed6fa46510a'
           ,1)
GO

INSERT INTO [dbo].[ProductCategories]
           ([CategoryName])
     VALUES
           ('Category1')
GO

-- Inserting 10 data into the Products table
INSERT INTO [dbo].[Products]
           ([ProductName]
           ,[ProductDescription]
           ,[Capacity]
           ,[ProductPrice]
           ,[ProductCategoryId])
     VALUES
           ('Organic Whole Milk','Certified organic whole milk, perfect for babies 1 year and older',1.0,12000,1),
           ('Low-Fat Milk','Low-fat milk for older toddlers and children',1.0,21000,1),
           ('Growth Milk','Milk designed for toddlers with added vitamins and minerals for healthy growth',1.0,33000,1),
           ('Goat Milk','Gentle goat milk for babies with sensitive stomachs',1.0,24000,1),
           ('Soy Milk','Plant-based soy milk for babies with allergies to cow’s milk',1.0,35000,1),
           ('Rice Milk','Hypoallergenic rice milk for babies with multiple allergies',1.0,60000,1),
           ('Almond Milk','Unsweetened almond milk, a healthy alternative to cow’s milk',1.0,89000,1),
           ('Formula Starter Pack','Starter pack of infant formula, ideal for first-time parents',0.9,57000,1),
           ('Formula Follow-Up Pack','Follow-up formula for babies 6 months and older',0.9,48000,1),
           ('Formula Toddler Pack','Toddler formula for children 1 year and older',0.9,65000,1);
GO

-- Inserting 4 data into the Carts table
INSERT INTO [dbo].[Carts]
           ([AccountId]
           ,[ProductId]
           ,[Quantity]
           ,[UnitPrice]
           ,[Status])
     VALUES
           (1,1,2,12000,1),  -- 2 units of Organic Whole Milk
           (1,3,1,33000,1),  -- 1 unit of Growth Milk
           (1,5,1,35000,1),  -- 1 unit of Soy Milk
           (1,9,1,48000,1); -- 1 unit of Formula Follow-Up Pack
GO
