# Information and Guidelines for Contributors
Thank you for contributing to the Ashampoo Translations Repository and making it even better. We appreciate every contribution, whether it's reporting issues, fixing bugs, or adding new components.

## Code of Conduct
Please make sure that you follow our [code of conduct](CODE_OF_CONDUCT.md)

## Minimal Prerequisites to Compile from Source

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  
## Pull Requests
- Your Pull Request must only consist of one topic. It is better to split Pull Requests with more than one feature or bug fix in seperate Pull Requests
- You must choose `develop` as the target branch
- All tests must pass
- You must include tests when your Pull Requests alters any logic. This also ensures that your feature will not break in the future. For more information on testing, see one of the following sections
- You should work on a separate branch with a descriptive name. The following naming convention can be used: `feature/my-new-feature` for new features and enhancements, `fix/my-bug-fix` for bug fixes.
- If your Pull Request introduces a new feature, label ith with the `feature` label, if it fixes a bug, label it with the `bug` label, if it is a documentation change, label it with the `docs` label and if it changes a test, label it with the `test` label.
- Small changes should only contain one commit. If you have several commits, squash them into one commit ([Rebase guide](https://docs.github.com/en/github/getting-started-with-github/about-git-rebase))
- You should rebase your branch onto dev
- Before working on a large change, it is recommended to first open an issue to discuss it with others
- If your Pull Request is still in progress, convert it to a draft Pull Request
- Your commit messages should follow the following format:
```
<Project name>: <short description> (<linked issue>) - <major/minor>
```
> If your change is neither a major nor a minor change, dont append anything

For example:
```
JsonFormat: Add support for arrays (#123) - minor
```
or:
```
Abstractions: Fixed typo in documentation (#123)
```
