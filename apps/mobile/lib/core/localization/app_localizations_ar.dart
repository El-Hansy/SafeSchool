// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for Arabic (`ar`).
class AppLocalizationsAr extends AppLocalizations {
  AppLocalizationsAr([String locale = 'ar']) : super(locale);

  @override
  String get appTitle => 'تطبيق المدرسة';

  @override
  String get accessDenied => 'لم يتم تعيين الوصول لهذا الدور.';

  @override
  String get updateRequired => 'يلزم تحديث التطبيق';

  @override
  String get offlineQueued => 'تم حفظ الإجراء دون اتصال';
}
