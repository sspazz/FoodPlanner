CREATE TABLE Recipes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL
);

CREATE TABLE Ingredients (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE RecipeIngredients (
    RecipeId INT,
    IngredientId INT,
    Quantity NVARCHAR(50),
    FOREIGN KEY (RecipeId) REFERENCES Recipes(Id),
    FOREIGN KEY (IngredientId) REFERENCES Ingredients(Id),
    PRIMARY KEY (RecipeId, IngredientId)
);

CREATE TABLE MealPlans (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Date DATE NOT NULL
);

CREATE TABLE MealPlanRecipes (
    MealPlanId INT,
    RecipeId INT,
    FOREIGN KEY (MealPlanId) REFERENCES MealPlans(Id),
    FOREIGN KEY (RecipeId) REFERENCES Recipes(Id),
    PRIMARY KEY (MealPlanId, RecipeId)
);
