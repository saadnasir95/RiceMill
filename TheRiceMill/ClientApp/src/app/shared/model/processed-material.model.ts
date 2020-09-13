export class ProcessedMaterial {
    id: number;
    productId: number;
    boriQuantity: number;
    bagQuantity: number;
    perKg: number;
    totalKg: number;
}
  

export class CreateProcessedMaterial {
    lotId: number;
    lotYear: number;
    processedMaterials: Array<ProcessedMaterial>;
}

