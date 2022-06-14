# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.2] - 2022-06-14
### Fixed
- Fixed IL2CPP builds stripping which were causing package initalization code to be stripped away

## [1.0.1] - 2022-03-03
### Changed
- Updated usage guide documentation.

## [1.0.0] - 2022-02-22
### Fixed
- Fixed counters sampling in Profiler-only capture mode.

## [1.0.0-pre.5] - 2022-02-04
### Fixed
- Fixed example script.

## [1.0.0-pre.4] - 2022-02-02
### Added
- Added usage guide documentation.

## [1.0.0-pre.3] - 2021-09-29
### Changed
- Reduced plugin binary size by ~10%.
- HWCP library is updated to the latest version to add Mali-G68 and Mali-G78 support.

## [1.0.0-pre.2] - 2021-09-29
### Fixed
- Fixed crash on some Mali-T880 devices caused by zero data in counters

## [1.0.0-pre.1] - 2021-09-17

### Changed
- Significantly reduced plugin binary size.
- HWCP library is updated to the latest version.
- Counters were renamed to be named uniformely.

### Added
- Exposed additional counters for relative load measurement.
- Exposed additional counters for primitives count and GPU shader core activity.
- Added API documentation.
- Added tooltips to Profiler Module UI.
- Added examples and usage guide into documentation.
- Added detailed description for each hardware counter.
- Added new play mode verification tests.

### Fixed
- Fixed issue when Pixels counter might have shown large negative value.
- Fixed issue that it was impossible to add counters before connecting Profiler to an Android device.

## [0.2.0] - 2021-06-16

### Support for more hardware counters and UX improvements
- Updated HWCP library to the latest version.
- Added custom Profiler Module "Mali System Metrics" with improved detailed view.
- Added more hardware and derivated counters. See "Available metrics" for more details.
- Improved metrics description documentation.

## [0.1.0] - 2021-03-01

### Initial release
- Added support for ARM Mali GPU counters
