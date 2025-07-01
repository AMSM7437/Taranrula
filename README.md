# Taranrula Search Engine

A modular search engine built with C#, ASP.NET Core, React.js, SQLite.
The entire thing is made to be modular and uses no libraries, you can just take any component and throw it wherever you want and it'll do its job. 

## Explanation
- The crawler takes a url to a page, downloads the html of this page and extracts the links from the html to crawl more. 
- Each downloaded html is passed to the parser which uses regex to extract all text from the html file and pass it to the indexer via the runner.
- The indexer takes the words and stores them in a reverse index SQLite database (for ease of use and setup).
- The API uses the same database to fetch the results based on relevance and frequency.
- It still requires a lot of work and a lot more features, so if you'd like to suggest anything please contact me.
---
## Notes
there's really nothing to it ,just make sure you do npm install for the react app before running, and if something does not work just check the server url.
i'll work on making that better to have a static API url.
---
## Built With

- C#
- ASP.NET Core
- React.
- SQLite
---


## Contact

If you have any questions, suggestions, or feedback:

- **GitHub Profile:** [AMSM7437](https://github.com/AMSM7437)
- **Email:** amsm7437@gmail.com