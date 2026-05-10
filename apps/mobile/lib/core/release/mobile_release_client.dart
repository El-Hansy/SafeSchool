import '../api/mobile_api_client.dart';

class MobileReleaseClient {
  const MobileReleaseClient(this.api);
  final MobileApiClient api;

  MobileRelease current({int versionCode = 1200}) => api.currentRelease(versionCode: versionCode);
}
