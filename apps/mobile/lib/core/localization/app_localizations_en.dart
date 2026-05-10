// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for English (`en`).
class AppLocalizationsEn extends AppLocalizations {
  AppLocalizationsEn([String locale = 'en']) : super(locale);

  @override
  String get appTitle => 'SafeSchool Mobile';

  @override
  String get accessDenied => 'Access is not assigned for this role.';

  @override
  String get updateRequired => 'Update required';

  @override
  String get offlineQueued => 'Offline action queued';
}
