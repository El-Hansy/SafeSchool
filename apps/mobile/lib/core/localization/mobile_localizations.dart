import 'package:flutter/widgets.dart';

class MobileLocalizations {
  const MobileLocalizations(this.languageCode);

  final String languageCode;

  bool get isArabic => languageCode == 'ar';
  TextDirection get direction => isArabic ? TextDirection.rtl : TextDirection.ltr;

  String text(String key) {
    final table = isArabic ? _ar : _en;
    return table[key] ?? _en[key] ?? key;
  }
}

const _en = <String, String>{
  'app.title': 'SafeSchool Mobile',
  'access.denied': 'Access is not assigned for this role.',
  'release.update': 'Update required',
  'sync.offline': 'Offline action queued',
};

const _ar = <String, String>{
  'app.title': 'تطبيق المدرسة',
  'access.denied': 'لم يتم تعيين الوصول لهذا الدور.',
  'release.update': 'يلزم تحديث التطبيق',
  'sync.offline': 'تم حفظ الإجراء دون اتصال',
};
