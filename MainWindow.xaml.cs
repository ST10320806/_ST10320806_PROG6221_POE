using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgPOE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public List<string> savedRecipeNames;
        public List<Ingredient> ingredients;
        public List<Recipe> recipes;

        public class Ingredient // a class to store the variables of the ingredients and for better practice
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string FoodGroup { get; set; }
            public int Calories { get; set; }
        }
        public class Recipe
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public string Steps { get; set; }

            // Method to calculate total calories in the recipe
            public int CalculateTotalCalories()
            {
                int totalCalories = 0;

                foreach (var ingredient in Ingredients)
                {
                    totalCalories += ingredient.Calories;
                }

                return totalCalories;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

       private void SaveRecipeName_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();

            // Check if the recipe name is not empty or whitespace
            if (!string.IsNullOrWhiteSpace(recipeName))
            {
                // Initialize the savedRecipeNames list if it's not already initialized
                if (savedRecipeNames == null)
                {
                    savedRecipeNames = new List<string>();
                }

                // Check if the recipe name already exists in the list to prevent duplicates
                if (savedRecipeNames.Contains(recipeName))
                {
                    MessageBox.Show($"The recipe name '{recipeName}' already exists. Please enter a different name.", "Duplicate Recipe Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    // Add the recipe name to the savedRecipeNames list
                    savedRecipeNames.Add(recipeName);

                    // Provide feedback to the user
                    MessageBox.Show($"Recipe name '{recipeName}' has been saved!", "Recipe Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear the RecipeNameTextBox for new input
                    

                    // Enable the user to continue entering ingredient details
                    // You can place focus on the next field (e.g., IngredientNameTextBox) to enhance UX
                    IngredientNameTextBox.Focus();

                    // Optionally: Update the state of related UI elements
                   
                }
            }
            else
            {
                // Provide feedback if the input is invalid
                MessageBox.Show("Please enter a recipe name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void SaveIngredient_Click(object sender, RoutedEventArgs e)
        {
            string name = IngredientNameTextBox.Text.Trim();
            string quantity = QuantityTextBox.Text.Trim();
            string unit = UnitTextBox.Text.Trim();
            string foodGroup = FoodGroupComboBox.SelectedItem != null ? (FoodGroupComboBox.SelectedItem as ComboBoxItem).Content.ToString() : string.Empty;
            string caloriesText = CaloriesTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(quantity) || string.IsNullOrWhiteSpace(unit) || string.IsNullOrWhiteSpace(foodGroup) || string.IsNullOrWhiteSpace(caloriesText))
            {
                MessageBox.Show("Please fill in all the ingredient details.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Parse the calories input
            if (!int.TryParse(caloriesText, out int calories))
            {
                MessageBox.Show("Please enter a valid number for calories.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new Ingredient object
            Ingredient newIngredient = new Ingredient
            {
                Name = name,
                Quantity = quantity,
                Unit = unit,
                FoodGroup = foodGroup,
                Calories = calories // Set the calories property
            };

            // Initialize the ingredients list if it's not already
            if (ingredients == null)
            {
                ingredients = new List<Ingredient>();
            }
            ingredients.Add(newIngredient);

            // Provide feedback to the user
            MessageBox.Show($"Ingredient '{name}' has been added!", "Ingredient Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear the input fields
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
            CaloriesTextBox.Clear();
        }

       

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {

            string recipeName = RecipeNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(recipeName))
            {
                MessageBox.Show("Please enter a recipe name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Step 2: Retrieve the recipe steps
            string recipeSteps = RecipeStepsTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(recipeSteps))
            {
                MessageBox.Show("Please enter the recipe steps.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Step 3: Validate that there are ingredients added
            if (ingredients == null || ingredients.Count == 0)
            {
                MessageBox.Show("Please add at least one ingredient.", "No Ingredients", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Step 4: Check if the recipes list is initialized
            if (recipes == null)
            {
                recipes = new List<Recipe>();
            }

            // Step 5: Create a new Recipe object and populate it
            Recipe newRecipe = new Recipe
            {
                Name = recipeName,
                Ingredients = new List<Ingredient>(ingredients), // Create a copy of the current list of ingredients
                Steps = recipeSteps
            };

            // Step 6: Add the new Recipe object to the recipes list
            recipes.Add(newRecipe);

            // Step 7: Provide feedback to the user
            MessageBox.Show($"Recipe '{recipeName}' has been saved successfully!", "Recipe Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            // Step 8: Clear the input fields for new entries
            RecipeNameTextBox.Clear();
            RecipeStepsTextBox.Clear();
            ingredients.Clear(); // Clear the list of ingredients for the next recipe

            // Optionally: Clear the individual ingredient input fields
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
            





        }
        private void UpdateRecipeList()
        {
            // This method can be used to update any UI element like a ListBox showing the current recipes
            //RecipesListBox.ItemsSource = null;
            //RecipesListBox.ItemsSource = recipes;
        }

        private void DisplayWindow_Click(object sender, RoutedEventArgs e)
        {
            
                DisplayWindow displayWindow = new DisplayWindow(recipes);
                displayWindow.Show();
            
        }
        
    }
}

