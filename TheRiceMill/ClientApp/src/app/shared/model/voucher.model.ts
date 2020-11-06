import { VoucherDetailType, VoucherType } from './enums';
import { VoucherDetail } from './voucher-detail.model';

export class Voucher {
  id: number;
  companyId: number;
  type: VoucherType;
  detailType: VoucherDetailType;
  voucherDetails: VoucherDetail[];
}
