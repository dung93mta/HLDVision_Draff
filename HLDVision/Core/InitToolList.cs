using HLDVision.Edit;
using HLDVision.Edit.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision.Core
{
    partial class HldToolBlockTree
    {
        #region Group name
        //const string grpAcqition = "Acqsition";
        const string grpImage = "Image";
        const string grpTool = "Tool";
        const string grpMake = "Make";
        const string grpMisc = "Misc";
        #endregion




        void InitToolList()
        {
            tools = new Dictionary<HldToolBase, HldToolEditForm>();

            //===========================================================================================================================
            tools.Add(new HldAcquisition() { Group = grpImage }, new HldToolEditForm(new HldAcqisitionEdit()));
            tools.Add(new HldRotation() { Group = grpImage }, new HldToolEditForm(new HldRotationEdit()));
            tools.Add(new HldSubImage() { Group = grpImage }, new HldToolEditForm(new HldSubImageEdit()));
            tools.Add(new HldEqualizer() { Group = grpImage }, new HldToolEditForm(new HldEqualizerEdit()));
            tools.Add(new HldFixture() { Group = grpImage }, new HldToolEditForm(new HldFixtureEdit()));
            tools.Add(new HldHistogram() { Group = grpImage }, new HldToolEditForm(new HldHistogramEdit()));
            tools.Add(new HldImageCalculate() { Group = grpImage }, new HldToolEditForm(new HldImageCalculateEdit()));
            tools.Add(new HldImageConverter() { Group = grpImage }, new HldToolEditForm(new HldImageConverterEdit()));
            tools.Add(new HldImageSave() { Group = grpImage }, new HldToolEditForm(new HldImageSaveEdit()));
            //===========================================================================================================================
            tools.Add(new HldBarcode() { Group = grpTool }, new HldToolEditForm(new HldBarcodeEdit()));
            tools.Add(new HldBlob() { Group = grpTool }, new HldToolEditForm(new HldBlobEdit()));
            tools.Add(new HldBlur() { Group = grpTool }, new HldToolEditForm(new HldBlurEdit()));
            tools.Add(new HldCameraCalibration() { Group = grpTool }, new HldToolEditForm(new HldCameraCalibrationEdit()));
            tools.Add(new HldDistance2P() { Group = grpTool }, new HldToolEditForm(new HldDistance2PEdit()));
            tools.Add(new HldDistance3P() { Group = grpTool }, new HldToolEditForm(new HldDistance3PEdit()));
            tools.Add(new HldEdge() { Group = grpTool }, new HldToolEditForm(new HldEdgeEdit()));
            tools.Add(new HldFindLine() { Group = grpTool }, new HldToolEditForm(new HldFindLineEdit()));
            tools.Add(new HldFindParallelLine() { Group = grpTool }, new HldToolEditForm(new HldFindParallelLineEdit()));
            tools.Add(new HldTemplateMatch() { Group = grpTool }, new HldToolEditForm(new HldTemplateMatchEdit()));
            tools.Add(new HldReadOCR() { Group = grpTool }, new HldToolEditForm(new HldReadOCREdit()));
            tools.Add(new HldToolBlock() { Group = grpTool }, new HldToolEditForm(new HldToolBlockEdit(hldToolTreeView)));
            tools.Add(new HldWarpping() { Group = grpTool }, new HldToolEditForm(new HldWarppingEdit()));
            tools.Add(new HldWarppingPoint() { Group = grpTool }, new HldToolEditForm(new HldWarppingPointEdit()));
            //===========================================================================================================================
            tools.Add(new HldMakeLattice() { Group = grpMake }, new HldToolEditForm(new HldMakeLatticeEdit()));
            tools.Add(new HldMakeLine() { Group = grpMake }, new HldToolEditForm(new HldMakeLineEdit()));
            tools.Add(new HldMakePoint() { Group = grpMake }, new HldToolEditForm(new HldMakePointEdit()));
            tools.Add(new HldMakeRectFromLine() { Group = grpMake }, new HldToolEditForm(new HldMakeRectFromLineEdit()));
            tools.Add(new HldMakeRegions() { Group = grpMake }, new HldToolEditForm(new HldMakeRegionsEdit()));
            tools.Add(new HldRegion() { Group = grpMake }, new HldToolEditForm(new HldRegionEdit()));
            //===========================================================================================================================
            tools.Add(new HldDataAnalysis() { Group = grpMisc }, new HldToolEditForm(new HldDataAnalysisEdit()));
            tools.Add(new HldDataLog() { Group = grpMisc }, new HldToolEditForm(new HldDataLogEdit()));
            tools.Add(new HldResultAnalysis() { Group = grpMisc }, new HldToolEditForm(new HldResultAnalysisEdit()));
            tools.Add(new HldMasking() { Group = grpMisc }, new HldToolEditForm(new HldMaskingEdit()));
            tools.Add(new HldMeasureSharpness() { Group = grpMisc }, new HldToolEditForm(new HldMeasureSharpnessEdit()));
            tools.Add(new HldMorphology() { Group = grpMisc }, new HldToolEditForm(new HldMorphologyEdit()));
            tools.Add(new HldSharpness() { Group = grpMisc }, new HldToolEditForm(new HldSharpnessEdit()));
            tools.Add(new HldIntersectLine() { Group = grpMisc }, new HldToolEditForm(new HldIntersectLineEdit()));
            tools.Add(new HldJudgement() { Group = grpMisc }, new HldToolEditForm(new HldJudgementEdit()));
            //===========================================================================================================================

        }
    }
}
