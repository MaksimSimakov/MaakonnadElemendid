using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Maakonnad
{
    public class MainPage : ContentPage
    {
        Picker statePicker, stateCapitalPicker;
        string[] statesNames = new string[] { "Harjumaa", "Hiiumaa", "Ida-Virumaa", "Järvamaa", "Jõgevamaa", "Läänemaa", "Lääne-Virumaa", "Pärnumaa", "Põlvamaa", "Raplamaa", "Saaremaa", "Tartumaa", "Valgamaa", "Viljandimaa", "Võrumaa" };
        string[] statesCapitalsNames = new string[] { "Tallinn", "Kärdla", "Jõhvi", "Paide", "Jõgeva", "Haapsalu", "Rakvere", "Pärnu", "Põlva", "Rapla", "Kuressaare", "Tartu", "Valga", "Viljandi", "Võru" };
        int[] statesPopulation = new int[] { 598059, 9387, 136240, 30286, 28734, 20507, 59325, 85938, 25006, 33311, 33108, 152977, 28370, 46371, 35782 };
        int[] stateCapitalsPopulation = new int[] { 445494, 3230, 10321, 8127, 5577, 9675, 15085, 50643, 5174, 5069, 13097, 93124, 12539, 17525, 11859 };
        double[] statesSurface = new double[] { 4326.7, 1032.44, 2971.55, 2674.14, 2544.86, 1815.57, 3695.72, 5418.73, 1823.34, 2765.06, 2937.64, 3349.3, 1917.09, 3420.04, 2773.14 };
        double[] stateCapitalsSurface = new double[] { 159.2, 4.5, 7.62, 10.036, 3.86, 10.59, 10.75, 32.22, 5.46, 4.67, 14.95, 16.54, 14.62, 13.24 };
        Label subjectName, subjectPopulation, subjectCenter, subjectSurface, subjectPopulationDensity;
        Image subjectPicture;
        Switch capitalSwitch;
        StackLayout subjectCenterLayout;
        public MainPage()
        {
            Title = "Уезды Эстонии";
            statePicker = new Picker() { ItemsSource = statesNames, 
                SelectedIndex = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            statePicker.SelectedIndexChanged += StatePicker_SelectedIndexChanged;
            stateCapitalPicker = new Picker() { ItemsSource = statesCapitalsNames, 
                SelectedIndex = 0, 
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            stateCapitalPicker.SelectedIndexChanged += StateCapitalPicker_SelectedIndexChanged;
            StackLayout pickerLayout = new StackLayout()
            {   Orientation = StackOrientation.Horizontal,
                Children = { statePicker, stateCapitalPicker }
            };
            // Subject info
            subjectPicture = new Image();
            subjectName = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 28,
                FontAttributes = FontAttributes.Bold
            };
            subjectPopulation = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand
            }; 
            subjectCenter = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            subjectSurface = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            subjectPopulationDensity = new Label()
            {
                HorizontalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            Button subjectWikiButton = new Button() { 
                Text = "Подробнее",  
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            subjectWikiButton.Clicked += SubjectWikiButton_Clicked;
            Label capitalSwitchLabel = new Label()
            {
                Text = "Отобразить данные о центре уезда"
            };
            capitalSwitch = new Switch()
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            capitalSwitch.Toggled += CapitalSwitch_Toggled;
            StackLayout capitalSwitchLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { capitalSwitchLabel, capitalSwitch },
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout subjectPopulationLayout = new StackLayout
            {
                Children = { new Label() { Text = "Население", FontAttributes = FontAttributes.Bold }, subjectPopulation },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            subjectCenterLayout = new StackLayout
            {
                Children = { new Label() { Text = "Административный центр", FontAttributes = FontAttributes.Bold }, subjectCenter },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout subjectSurfaceLayout = new StackLayout
            {
                Children = { new Label() { Text = "Площадь", FontAttributes = FontAttributes.Bold }, subjectSurface },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout subjectPopulationDensityLayout = new StackLayout
            {
                Children = { new Label() { Text = "Плотность населения", FontAttributes = FontAttributes.Bold}, subjectPopulationDensity },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout subjectLayout = new StackLayout
            {
                Children = { subjectPicture, subjectName, subjectPopulationLayout, subjectCenterLayout, subjectSurfaceLayout, subjectPopulationDensityLayout, subjectWikiButton },
            };
            StackLayout stackLayout = new StackLayout() { 
                Children = { pickerLayout, capitalSwitchLayout, subjectLayout },
                Margin = new Thickness(20)
            };
            Content = stackLayout;
            DisplayInfo(capitalSwitch.IsToggled, statePicker.SelectedIndex);
        }

        private async void SubjectWikiButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://et.m.wikipedia.org/wiki/" + subjectName.Text);
        }

        private void CapitalSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Switch capitalSwitch = sender as Switch;
            if (capitalSwitch.IsToggled) 
            {
                DisplayInfo(capitalSwitch.IsToggled, stateCapitalPicker.SelectedIndex);
            }
            else
            {
                DisplayInfo(capitalSwitch.IsToggled, statePicker.SelectedIndex);
            }
        }

        private void StateCapitalPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (statePicker.SelectedIndex != picker.SelectedIndex)
            {
                statePicker.SelectedIndex = picker.SelectedIndex;
            };
            DisplayInfo(capitalSwitch.IsToggled, picker.SelectedIndex);
            
        }

        private void StatePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (stateCapitalPicker.SelectedIndex != picker.SelectedIndex)
            {
                stateCapitalPicker.SelectedIndex = picker.SelectedIndex;
            }
            DisplayInfo(capitalSwitch.IsToggled, picker.SelectedIndex);
        }

        private void DisplayInfo(bool isStateCapital, int index)
        {
            int population;
            double surface, populationDensity;
            string name;
            if (isStateCapital)
            {
                population = stateCapitalsPopulation[index];
                surface = stateCapitalsSurface[index];
                name = statesCapitalsNames[index];
                subjectCenterLayout.IsVisible = false;
                subjectPicture.Source = GetSubjectImage(statesNames[index]);
            }
            else
            {
                population = statesPopulation[index];
                surface = statesSurface[index];
                name = statesNames[index];
                subjectCenterLayout.IsVisible = true;
                subjectCenter.Text = statesCapitalsNames[index];
                subjectPicture.Source = GetSubjectImage(name);
            }
            populationDensity = CalculatePopulationDensity(population, surface);
            subjectName.Text = name;
            subjectPopulation.Text = population.ToString();
            subjectSurface.Text = surface.ToString();
            subjectPopulationDensity.Text = populationDensity.ToString() + " чел/кв. км.";
        }

        private ImageSource GetSubjectImage(string subjectName)
        {
            char[] charsForReplace = new char[] { 'ä', 'õ', 'ü', 'ö', '-'};
            char[] charsToReplace = new char[] { 'a', 'o', 'y', 'o', '_'};
            for (int i = 0; i < charsForReplace.Length; i++)
            {
                subjectName = subjectName.Replace(charsForReplace[i], charsToReplace[i]);
            }
            return ImageSource.FromFile(subjectName + ".png");
        }

        private double CalculatePopulationDensity(int population, double surface)
        {
            return Math.Round(population / surface, 2);
        }
    }
}