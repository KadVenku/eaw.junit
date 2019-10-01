# eaw.junit

Works in conjunction with GlyphXTools' ModCheck application and transforms the tools output into a JUnit conform xml file.

Exemplary usage within the context of a GitLab CI/CD piepline.

```yaml
test-job:
    stage: test
    <<: *art-process
    script:
      - if exist .build\ (del /F/Q/S .build\)
      - if not exist .build\ (mkdir .build\)
      - if not exist .build\test-results\ (mkdir .build\test-results\)
      - bin\ModCheck -FOC 2> .build\checkmod.report
      - dotnet bin\eawunit.dll WARNING ".build\checkmod.report" ".build\test-results"
    artifacts:
      reports:
        junit: .build\test-results\TEST-*.xml
```

This generates a JUnit conform xml that GitLab can use to display test results on pielines for Merge Requests.

## Requirements

* dotnet SDK 2.1
* dotnet Runtime 2.1
