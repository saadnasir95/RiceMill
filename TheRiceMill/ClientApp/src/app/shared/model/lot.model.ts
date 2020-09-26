import { ProcessedMaterial } from "./processed-material.model";
import { Gatepass } from "./gatepass.model";
import { Stock } from "./stock-in.model";

export class Lot {
    id: number;
    year: string;
    processedMaterials: Array<ProcessedMaterial>;
    gatePasses: Array<Gatepass>;
    rateCosts: [];
    stockIns: Array<Stock>;
    stockOuts: Array<Stock>;
}