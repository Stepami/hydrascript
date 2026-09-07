# Testing follow-up

Read this before changing executable code/tests or choosing validation commands. Use the applicable Rider and .NET skills required by [AGENTS.md](../AGENTS.md).

## Detect the runner

The checked-in configuration is SDK-style .NET 10, **xUnit on Microsoft.Testing.Platform (MTP)**, in native MTP `dotnet test` mode. [global.json](../global.json) selects the runner; [tests/Directory.Build.props](../tests/Directory.Build.props) enables the xUnit MTP runner and executable test projects. The current package is `xunit.v3.mtp-v2`; its version is centrally managed and should be read from [Directory.Packages.props](../Directory.Packages.props).

Use `dotnet-test:platform-detection` and `dotnet-test:run-tests` before selecting commands; use `dotnet-test:filter-syntax` for unfamiliar filters. Recheck configuration after dependency/SDK changes.

## Where tests live

| Project | Responsibility | Conventions |
| --- | --- | --- |
| [HydraScript.UnitTests](../tests/HydraScript.UnitTests) | Domain and Application behavior | Assembly-wide `Category=Unit`; xUnit, AwesomeAssertions, NSubstitute, AutoFixture |
| [LexerRegexGenerator.UnitTests](../tests/HydraScript.Infrastructure.LexerRegexGenerator.UnitTests) | Generated pattern source and ordering | Explicit `Category=Unit`; Roslyn GeneratorDriver and xUnit assertions |
| [HydraScript.IntegrationTests](../tests/HydraScript.IntegrationTests) | Real interpreter pipeline, success/error programs, console/input/dumps | TestHostFixture composes production services and substitutes external dependencies |

Before locating C# tests, use `rider-skills:finding-tests` and its documented Rider `findTests` route. Read the returned tests and fixture before adding coverage. Follow the skill's fallback rules when tooling is unavailable.

## Writing tests

- Use `dotnet-test:code-testing-agent` for test implementation and `dotnet-test:assertion-quality` for assertions; use the relevant gap-analysis skill when the task is specifically about missing coverage.
- Follow `MethodName_Scenario_ExpectedBehavior` (Roy Osherove). The happy-path convention requested in issue #205 is `MethodName_Always_Success`; prefer a specific scenario/outcome when it adds information.
- Add a minimal regression at the stage that owns the defect. For language behavior, also verify the interpreter's observable result where unit tests cannot establish it.
- Reuse [AutoHydraScriptDataAttribute](../tests/HydraScript.UnitTests/AutoHydraScriptDataAttribute.cs) for applicable unit fixtures and [TestHostFixture](../tests/HydraScript.IntegrationTests/TestHostFixture.cs) for pipeline tests. Keep semantic inputs explicit when randomized data would hide the case.
- Use a disposable runner from `fixture.GetRunner(new TestHostFixture.Options(InMemoryScript: script))`. Assert the appropriate `Executor.ExitCodes` and relevant output/diagnostics, as in [ArithmeticTests](../tests/HydraScript.IntegrationTests/SuccessPrograms/ArithmeticTests.cs) and [FunctionWithoutReturnStatementTests](../tests/HydraScript.IntegrationTests/ErrorPrograms/FunctionWithoutReturnStatementTests.cs).
- [SuccessfulProgramsTests](../tests/HydraScript.IntegrationTests/SuccessPrograms/SuccessfulProgramsTests.cs) automatically enumerates files directly in `Samples/`; those cases assert successful exit only. A new sample joins that smoke suite, but does not establish expected output. Add a focused assertion for behavior-sensitive regressions. Samples are copied to test output with `PreserveNewest`.
- Fixture defaults mock files and environment variables; sample tests explicitly use real ones. Avoid introducing machine-dependent state, input waits, or real environment mutation into new cases.
- New diagnostics need targeted negative cases; do not assume the existing sample suite covers them.

## Commands from the repository root

Prefer Rider build/run tools for development checks and focused tests. Use Rider's terminal for the following CLI/CI commands when needed. A build start is not a result: poll its returned session; record test exit status and actual executed counts.

```powershell
dotnet --version
dotnet restore ExtendedJavaScriptSubset.slnx
dotnet build ExtendedJavaScriptSubset.slnx --no-restore -c Debug
dotnet test --solution ExtendedJavaScriptSubset.slnx -c Debug --no-build
```

Run unit-category tests (includes generator tests):

```powershell
dotnet test --solution ExtendedJavaScriptSubset.slnx -c Debug --no-build --filter-trait "Category=Unit"
```

Run one class or the integration project:

```powershell
dotnet test --project tests/HydraScript.UnitTests -c Debug --filter-class "HydraScript.UnitTests.Application.TypeDeclarationsResolverTests"
dotnet test --project tests/HydraScript.IntegrationTests -c Debug
```

Native MTP uses `--project`/`--solution` and passes MTP flags directly. xUnit filters use `--filter-class`, `--filter-method`, or `--filter-trait`. VSTest's `--filter FullyQualifiedName=...`, `--logger trx`, and `--collect` are not the commands for this setup.

Only use `--no-build` after matching outputs were built. The shared test props ignore MTP exit code 8 (no tests); therefore a successful exit alone does not prove a selected test ran. Verify a nonzero count for the intended project/class; an empty nonmatching project in a solution-wide trait run can be expected.

## Coverage and completion

The integration project references the MTP CodeCoverage extension. Do not pass `--coverage` to the whole solution: the other test projects do not register that extension.

```powershell
dotnet test --project tests/HydraScript.IntegrationTests -c Debug --no-build --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml --coverage-settings tests/coverage-exclude.xml
```

[PR CI](../.github/workflows/pr.yml) runs unit-category tests and integration coverage on Ubuntu, then requires **80% changed-line coverage** against `origin/master` using `diff-cover`. It reads `TestResults/coverage.cobertura.xml`. The threshold is not an overall project percentage. [Push CI](../.github/workflows/push.yml) runs all tests on Windows; [master CI](../.github/workflows/master.yml) collects integration coverage on Windows.

Respect [coverage-exclude.xml](../tests/coverage-exclude.xml). Do not expand exclusions simply to pass the gate. Local coverage collection alone does not verify the CI diff threshold.

For behavior changes, run the focused regression first, then affected project tests; run the full suite for changes crossing pipeline stages or shared infrastructure. Check Rider diagnostics and build where applicable. Pure documentation changes need link/path, structure, and formatting validation, without running the interpreter suite. For an isolated semantic refactor, follow the Rider skill's validation rules; behavior changes still need the relevant checks.

For performance tasks, use [HydraScript.Benchmarks](../benchmarks/HydraScript.Benchmarks) and report configuration, SDK/runtime, workload, timing, and allocations. The current harness shuffles samples and processes a subset while reusing services; control those variables before attributing a performance difference. Benchmark numbers are not correctness tests.

Report checks actually run, passed/failed counts, skipped or unmatched tests, and any limitations.