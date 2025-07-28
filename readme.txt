Name: Md Amir Habib
Universal News Feed Application - README

A. How to Build and Run the Application

* Open the solution file (.sln) in Visual Studio.
* Restore NuGet packages if not already restored (e.g., HtmlAgilityPack).
* Build the solution.
* Run the application.

B. Issues Faced and Potential Improvements

* I had limited recent experience with C#, so I revisited core concepts, especially WPF and MVVM architecture.
* I referred to tutorial videos, official documentation, and AI tools to build foundational understanding.
* If given more time, I would:

  * Make the application fully asynchronous for better performance when fetching data from multiple sources.
  * Add features such as opening articles in a dedicated window.
  * Implement pagination for better navigation and user experience.

C. Optional Features / Improvements Included

* Automatically loads articles on startup.
* Filters out duplicate articles based on headline and URL.
* Supports multiple news sources through a configurable JSON file.
* Allows users to click on headlines to open articles in the browser.
* Applies a clean MVVM pattern using ICommand for button interactions.

D. How I Tackled the Assignment

* Outlined the core functionality: fetching and displaying articles from multiple websites.
* Used MVVM architecture to maintain clear separation between UI and logic.
* Introduced a Services layer that uses interfaces and OOP principles:

  * Each website has its own parser logic encapsulated in a service class (e.g., MediumService).
  * This allows easy integration of additional sources in the future.
* Later, shifted to a configuration-based approach:

  * Created a JSON file to store each site’s base URL and XPath expressions for titles and links.
  * Now, new websites can be supported just by updating the config file—no code changes required.

E. Feedback

* This assignment helped solidify my understanding of MVVM design and WPF application development.
* I learned how to work with external data sources using HtmlAgilityPack.
* The structure made it easy to maintain and scale the app.
* With more time, I’d like to:

  * Add async programming.
  * Improve UI/UX polish.
  * Introduce unit tests for reliability.
* Overall, it was a valuable and rewarding learning experience.
