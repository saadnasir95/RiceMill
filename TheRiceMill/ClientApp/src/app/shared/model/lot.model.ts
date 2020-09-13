import { ProcessedMaterial } from "./processed-material.model";
import { Gatepass } from "./gatepass.model";

export class Lot {
    id: number;
    processedMaterials: Array<ProcessedMaterial>;
    gatePasses: Array<Gatepass>;
    rateCosts: [];
    stockIns: any;
    stockOuts: [];
    year: string;
}