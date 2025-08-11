
# Universal News Feed App

A cross-platform news feed application leveraging MVVM architecture and async data fetching to display, save, and manage news articles from multiple configurable sources. Originally built with WPF, this project can be extended to .NET MAUI for cross-platform UI support.

---

## Overview

The core of the application is the `MainViewModel`, which manages fetching news articles from configured sources, displaying them in a collection, and providing commands to refresh, open URLs, save articles to a database, and delete selected articles.

---

## Features

- **Asynchronous fetching** of news articles from multiple sources based on JSON configuration.
- **Observable collection** of selectable news articles for UI binding.
- **Command bindings** for:
  - Refreshing the news feed (`RefreshCommand`)
  - Opening news article URLs (`OpenUrlCommand`)
  - Deleting selected articles (`DeleteSelectedCommand`)
  - Saving articles to a persistent database (`SaveArticlesCommand`)
- **Loading state** tracking with an `IsLoading` property for UI feedback.
- **Robust error handling** during fetch, save, and delete operations.
- **Supports offline caching** by loading articles from a local database on startup.

---

## Architecture

The ViewModel depends on the following service interfaces, which enable platform-independent implementations:

- `IUrlOpenerService` — Opens URLs in the platform browser.
- `IConfigService` — Loads and parses source configurations.
- `IHtmlLoader` — Handles HTML content retrieval.
- `INewsArticleDbService` — Provides data persistence for articles.

This design facilitates easy porting to other platforms (e.g., MAUI) by providing platform-specific implementations of these services.

---

## Usage

### Instantiating the ViewModel

```csharp
var viewModel = new MainViewModel(
    urlOpenerService,
    configService,
    htmlLoader,
    newsArticleDbService);
