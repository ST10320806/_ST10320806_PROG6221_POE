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
/// Jesse Weeder
/// ST10320806
/// Module PROG6221
/// </summary>
    
    public partial class MainWindow : Window
    {

        //Declaring arrays
        public List<string> savedRecipeNames;
        public List<Ingredient> ingredients;
        public List<Recipe> recipes;

        public class Ingredient // A class to store the variables of the ingredients
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string FoodGroup { get; set; }
            public int Calories { get; set; }
        }
        public class Recipe //A class which stores the variables of the recipe
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public string Steps { get; set; }

            
            public int CalculateTotalCalories() //Method which calculates the total calories in a recipe
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
        private void SaveIngredient_Click(object sender, RoutedEventArgs e)//Event handler for the save ingredient button
        {
            //Caoturing values entered into the textboxes
            string name = IngredientNameTextBox.Text.Trim();
            string quantity = QuantityTextBox.Text.Trim();
            string unit = UnitTextBox.Text.Trim();
            string foodGroup = FoodGroupComboBox.SelectedItem != null ? (FoodGroupComboBox.SelectedItem as ComboBoxItem).Content.ToString() : string.Empty;
            string caloriesText = CaloriesTextBox.Text.Trim();

            //Checking if any textboxes are empty 
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(quantity) || string.IsNullOrWhiteSpace(unit) || string.IsNullOrWhiteSpace(foodGroup) || string.IsNullOrWhiteSpace(caloriesText))
            {
                MessageBox.Show("Please fill in all the ingredient details.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           
            if (!int.TryParse(caloriesText, out int calories))
            {
                MessageBox.Show("Please enter a valid number for calories.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Creating a newIngredient object
            Ingredient newIngredient = new Ingredient
            {
                Name = name,
                Quantity = quantity,
                Unit = unit,
                FoodGroup = foodGroup,
                Calories = calories
            };

            // Initialize the ingredients list
            if (ingredients == null)
            {
                ingredients = new List<Ingredient>();
            }
            ingredients.Add(newIngredient);

            MessageBox.Show($"Ingredient '{name}' has been added!", "Ingredient Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            // Clearing the input fields after completion
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
            CaloriesTextBox.Clear();
        }

       

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)//Event handler for the Save Recipe button
        {

            string recipeName = RecipeNameTextBox.Text.Trim();//capturing the value in the text box
            //Checking if the field is empty
            if (string.IsNullOrWhiteSpace(recipeName))
            {
                MessageBox.Show("Please enter a recipe name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Capturing the steps from the text box
            string recipeSteps = RecipeStepsTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(recipeSteps))
            {
                MessageBox.Show("Please enter the recipe steps.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (ingredients == null || ingredients.Count == 0)
            {
                MessageBox.Show("Please add at least one ingredient.", "No Ingredients", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (recipes == null)
            {
                recipes = new List<Recipe>();
            }

            // Step 5: Creating and populating a new recipe object
            Recipe newRecipe = new Recipe
            {
                Name = recipeName,
                Ingredients = new List<Ingredient>(ingredients), // Create a copy of the current list of ingredients
                Steps = recipeSteps
            };

            //Adding new object to the recipe list
            recipes.Add(newRecipe);

            MessageBox.Show($"Recipe '{recipeName}' has been saved successfully!", "Recipe Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            //Clearing fields after completion
            RecipeNameTextBox.Clear();
            RecipeStepsTextBox.Clear();
            ingredients.Clear();
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
            
        }       
        private void DisplayWindow_Click(object sender, RoutedEventArgs e)//Event handler for the display window button
        {
            
                DisplayWindow displayWindow = new DisplayWindow(recipes);
                displayWindow.Show();//Opening the new window
            
        }
        
    }
}

