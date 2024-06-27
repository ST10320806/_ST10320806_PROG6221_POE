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
using System.Windows.Shapes;
using static ProgPOE.MainWindow;

namespace ProgPOE
{    
    public partial class DisplayWindow : Window
    {
        private List<Recipe> _recipes;

        public DisplayWindow(List<Recipe> recipes)
        {
            InitializeComponent();
            _recipes = recipes;
            PopulateRecipeComboBox(recipes);
        }

        private void PopulateRecipeComboBox(List<Recipe> recipes)//Populating combobox with recipes from recipe list
        {
            //error handling checking if there is a recipe before trying to display
            if (recipes == null || recipes.Count == 0)
            {
                MessageBox.Show("No recipes available to display.", "No Recipes", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Order the recipes by their name in alphabetical order
            var sortedRecipeNames = recipes.Select(r => r.Name).OrderBy(name => name).ToList();

            //Bind the sorted recipe names to the ComboBox
            RecipeComboBox.ItemsSource = sortedRecipeNames;
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            //Getting values from the fields
            string filterIngredient = FilterIngredientTextBox.Text.Trim().ToLower();
            string filterFoodGroup = ((ComboBoxItem)FilterFoodGroupComboBox.SelectedItem)?.Content.ToString();
            int.TryParse(FilterMaxCaloriesTextBox.Text.Trim(), out int filterMaxCalories);

            //Applying the filters
            IEnumerable<string> filteredRecipeNames = _recipes.Select(r => r.Name);

            if (!string.IsNullOrWhiteSpace(filterIngredient))//Filtering recipes by ingredient name
            {
                filteredRecipeNames = filteredRecipeNames.Where(r => _recipes.Any(recipe =>
                    recipe.Name == r && recipe.Ingredients.Any(ingredient => ingredient.Name.ToLower().Contains(filterIngredient))));
            }

            if (filterFoodGroup != "All")//Filterin recipes by their food group
            {
                filteredRecipeNames = filteredRecipeNames.Where(r => _recipes.Any(recipe =>
                    recipe.Name == r && recipe.Ingredients.Any(ingredient => ingredient.FoodGroup == filterFoodGroup)));
            }

            if (filterMaxCalories > 0)//Filtering recipes by their calories
            {
                filteredRecipeNames = filteredRecipeNames.Where(r => _recipes.Any(recipe =>
                    recipe.Name == r && recipe.CalculateTotalCalories() <= filterMaxCalories));
            }

            //Updating combobox with the filtered recipes
            RecipeComboBox.ItemsSource = filteredRecipeNames.ToList();
        }

        private void ShowRecipeDetails_Click(object sender, RoutedEventArgs e)//Event handler for the show recipe details button
        {
            if (RecipeComboBox.SelectedItem != null)//Checking if an item has been selected from the combobox
            {
                string selectedRecipeName = RecipeComboBox.SelectedItem.ToString();
                Recipe selectedRecipe = _recipes.FirstOrDefault(r => r.Name == selectedRecipeName);

                if (selectedRecipe != null)
                {
                    //Display the details of the selected recipe
                    DisplayRecipe(selectedRecipe);
                }
            }
        }

        private void DisplayRecipe(Recipe recipe)//method for displaying the recipe selected in the combobox
        {
            //code for displaying the recipe in a certain structure
            StringBuilder details = new StringBuilder();
            details.AppendLine($"Recipe Name: {recipe.Name}");
            details.AppendLine("Ingredients:");
            foreach (var ingredient in recipe.Ingredients)
            {
                details.AppendLine($"- {ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}, Calories: {ingredient.Calories}");
            }
            details.AppendLine();
            details.AppendLine("Steps:");
            details.AppendLine(recipe.Steps);

            RecipeDetailsTextBlock.Text = details.ToString();
        }
    }
}
