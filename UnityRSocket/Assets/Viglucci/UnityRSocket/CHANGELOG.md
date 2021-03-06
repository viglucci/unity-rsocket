## [0.4.2](https://github.com/viglucci/unity-rsocket/compare/v0.4.1...v0.4.2) (2022-04-17)


### Bug Fixes

* include original code and message in stream errors for requestResponse + requestStream ([afd3435](https://github.com/viglucci/unity-rsocket/commit/afd343528d8c86d2f570bd0ce073645c0b389eda))

## [0.4.1](https://github.com/viglucci/unity-rsocket/compare/v0.4.0...v0.4.1) (2022-04-07)


### Bug Fixes

* set metadata flag on RequestStream and FireAndForget when metadata present ([b2a4ac6](https://github.com/viglucci/unity-rsocket/commit/b2a4ac69865c7e96d395300a68068045f901e3fd))

# [0.4.0](https://github.com/viglucci/unity-rsocket/compare/v0.3.1...v0.4.0) (2022-04-06)


### Features

* add Route to CompositeMetadataBuilder ([5c2b477](https://github.com/viglucci/unity-rsocket/commit/5c2b477aa0c82114c36e899fadb51f432088a403))

## [0.3.1](https://github.com/viglucci/unity-rsocket/compare/v0.3.0...v0.3.1) (2022-04-05)


### Bug Fixes

* remove debug line in KeepAlive timeout check handler ([a21cf87](https://github.com/viglucci/unity-rsocket/commit/a21cf87732efb9809c78c8011e7bfe12237a68b4))

# [0.3.0](https://github.com/viglucci/unity-rsocket/compare/v0.2.1...v0.3.0) (2022-04-05)


### Bug Fixes

* release action config ([f02609d](https://github.com/viglucci/unity-rsocket/commit/f02609d52055bc30f1b0385adab67100023011e5))


### Features

* add releaserc and github action for semantic releases ([0469118](https://github.com/viglucci/unity-rsocket/commit/04691181d0c3a3e133d6f8d244a2fb2b44cda439))

# 0.2.0

**Full Changelog**: https://github.com/viglucci/unity-rsocket/compare/0.2.0...0.2.1

fix: send KeepAlive frame in response to KeepAlive with (R) Respond flag set

# 0.1.0

### Features

- Keepalive
- Routing Extension
- Composite Metadata API
- TCP Client
- Fire & Forget
- Request Response
- Request Stream
