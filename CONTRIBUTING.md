# Contribution Guidelines

Contributions are welcomed! Here's a few things to know:

- [Contribution Guidelines](#contribution-guidelines)
  - [Steps to Contributing](#steps-to-contributing)
  - [Code of Conduct](#code-of-conduct)
      - [Do not point fingers](#do-not-point-fingers)
      - [Provide code feedback based on evidence](#provide-code-feedback-based-on-evidence)
      - [Ask questions do not give answers](#ask-questions-do-not-give-answers)
  - [HydraScript Contributor License Agreement](#hydrascript-contributor-license-agreement)

## Steps to Contributing

Here are the basic steps to get started with your first contribution. Please reach out with any questions.
1. Use [open issues](https://github.com/stepami/extended-js-subset/issues) to discuss the proposed changes. Create an issue describing changes if necessary to collect feedback. Also, please use provided labels to tag issues so everyone can easily sort issues of interest.
1. [Fork the repo](https://help.github.com/articles/fork-a-repo/) in order if you want to make and test local changes.
1. Create a new branch **from master** for the issue. We suggest prefixing the branch with type of contribution (`bugfix`/`feature`), your username and then a descriptive title: (e.g. `bugfix/user1/object-comparision` or `feature/user2/variable-initialization-check`)
1. Make code changes.
1. Ensure unit tests pass and code coverage minimum is satisfied.
1. Create a pull request against **master** branch.

## Code of conduct

Apart from the official [Code of Conduct](CODE_OF_CONDUCT.md) it is highly recommended to adopt the following behaviors, to ensure a great working environment:

#### Do not point fingers
Let’s be constructive.

<details>
<summary><em>Click here to see some examples</em></summary>

"This method is missing xmldoc" instead of "YOU forgot to put xmldoc".

</details>

#### Provide code feedback based on evidence 

When making code reviews, try to support your ideas based on evidence (papers, library documentation, stackoverflow, etc) rather than your personal preferences. 

<details>
<summary><em>Click here to see some examples</em></summary>

"When reviewing this code, I saw that the constructor of `MyClass` is fulfilled with many untrivial operations. However, [Microsoft's official guideline on constructor design](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/constructor) says that we should do minimal work in the constructor. We should follow the standard in the industry."

</details>

#### Ask questions do not give answers
Try to be empathic. 

<details>
<summary><em>Click here to see some examples</em></summary>

* Would it make more sense if ...?
* Have you considered this ... ?

</details>

## HydraScript Contributor License Agreement

Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once in this repo.
