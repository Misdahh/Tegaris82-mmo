# tegaris82 — GitHub Build

Unity version: **6000.0.43f1**.

## GitHub Actions

Workflows are included in `.github/workflows/`:

- `unity-android.yml` — builds an Android APK and uploads it as a GitHub Actions artifact.
- `unity-webgl.yml` — builds a WebGL package and uploads it as an artifact.

### Required GitHub Secrets

Add these repository secrets:

- `UNITY_EMAIL`
- `UNITY_PASSWORD`
- `UNITY_LICENSE`

Use a Unity license suitable for CI according to Unity's current licensing terms.

## App identity

- App name: **tegaris82**
- Android package: `com.tegaris82.mmo`
- Version: `1.0.0`

## Icon

The icon files are under `Assets/Branding/`. `BrandingBuild.cs` applies the icon during Unity editor/build preprocessing.

## Build locally

Open the project with Unity **6000.0.43f1**, then use `tegaris82 > Apply App Branding` before building if needed.
