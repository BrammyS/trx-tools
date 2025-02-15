<div id="top"></div>

<!-- Readme template from https://github.com/othneildrew/Best-README-Template -->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Apache License 2.0 License][license-shield]][license-url]



<div align="center">

<h1 align="center">trx-tools</h3>

  <p align="center">
    Reporting and merging tool for TRX files.
    <br />
    <a href="https://trx-tools.brammys.com">Demo Report</a>
    ·
    <a href="https://github.com/BrammyS/trx-tools/wiki">Explore the docs</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#example">Example</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

trx-tools is a project that aims to provide a set of tools to interact with TRX test files. For example, converting a TRX file to a HTML report, PDF report, Json file, etc. This also includes merging multiple TRX files into one.
See an example report [here](https://trx-tools.brammys.com).

The goals of this project is to provide a tool that can be used to generate test reports after using the `dotnet test` command. This command does not return a single report. Meaning, the trx files have to be merged manually after which they can be converted into a readable report.

### Example
![image](https://github.com/user-attachments/assets/b0a2246b-4071-4657-bdd3-a6d265b3bc5b)


### Built With

* [.NET](https://dotnet.microsoft.com/en-us/)
* [Scrutor](https://github.com/khellang/Scrutor)



<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.
1. Go to the latest [release](https://github.com/BrammyS/trx-tools/releases).
2. Download the correct runtime for your system from the assets of the release.
3. And you are ready to go! 


<!-- USAGE EXAMPLES -->
## Usage

The report tool has a help command that can be used to see all the available commands and options. 
```sh
trx-tools.Reporting help
```

To generate a report from a TRX file, use the following command:
```sh
trx-tools.Reporting html "path/to/trxFolder" "path/to/output/file.html"
```


<!-- ROADMAP -->
## Roadmap

- [x] Merging multiple TRX files
- [x] Generating a HTML report
- [x] Automatic release pipeline from tags
- [ ] Generating a PDF report

See the [open issues](https://github.com/BrammyS/trx-tools/issues) for a full list of proposed features (and known issues).




<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request




<!-- LICENSE -->
## License

Distributed under the Apache 2.0 License. See `LICENSE` for more information.


<p align="right">(<a href="#top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/BrammyS/trx-tools.svg?style=for-the-badge
[contributors-url]: https://github.com/BrammyS/trx-tools/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/BrammyS/trx-tools.svg?style=for-the-badge
[forks-url]: https://github.com/BrammyS/trx-tools/network/members
[stars-shield]: https://img.shields.io/github/stars/BrammyS/trx-tools.svg?style=for-the-badge
[stars-url]: https://github.com/BrammyS/trx-tools/stargazers
[issues-shield]: https://img.shields.io/github/issues/BrammyS/trx-tools.svg?style=for-the-badge
[issues-url]: https://github.com/BrammyS/trx-tools/issues
[license-shield]: https://img.shields.io/github/license/BrammyS/trx-tools.svg?style=for-the-badge
[license-url]: https://github.com/BrammyS/trx-tools/blob/master/LICENSE
