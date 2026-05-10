class InstallEventReporter {
  final List<String> _events = [];
  List<String> get events => List.unmodifiable(_events);

  void recordLaunch(String versionName) {
    _events.add('launch:$versionName');
  }
}
