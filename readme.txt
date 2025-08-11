# Universal News Feed App

Universal News Feed App is a cross-platform news aggregator application built using the MVVM pattern. It fetches news articles asynchronously from multiple configurable sources, displays them in a user-friendly interface, and allows users to save and manage articles locally.

The core functionality is powered by a robust `MainViewModel` that handles fetching, opening, saving, and deleting news articles through well-defined service interfaces. This architecture promotes easy portability across platforms such as WPF and .NET MAUI by isolating platform-specific implementations behind service abstractions.

---

## Key Features
- Fetch news asynchronously from multiple sources based on JSON configuration.
- Display articles with headline, source, and download timestamp.
- Open article URLs in the default browser.
- Save articles locally to a database.
- Delete selected articles.
- Loading indicators and user-friendly status messages.

---

This project demonstrates clean MVVM design, async programming best practices, and modular architecture suitable for desktop and mobile app development.
