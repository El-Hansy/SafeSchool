import 'mobile_release_client.dart';

class MobileVersionGuard {
  const MobileVersionGuard(this.releaseClient);
  final MobileReleaseClient releaseClient;

  bool canContinue(int versionCode) => !releaseClient.current(versionCode: versionCode).updateRequired;
}
