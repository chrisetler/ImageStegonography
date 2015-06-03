using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImageStenography
{
    public partial class ChageOfBitsTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void convertBits(object sender, EventArgs e)
        {
            try
            {
                byte inputByte = byte.Parse(inputNumberConcealing.Text);
                byte bitsOfOutput = (byte)(encodingBits.SelectedIndex);
                //to convert from N0 to N bits of precision:
                //get encoding coefficent by left shifting 11111111 N times
                byte encodingCoefficient = (byte)(((byte)255) << bitsOfOutput);

                //AND the encoding coefficient with the inputByte
                inputByte = (byte)(inputByte & encodingCoefficient);

                //Get the compressed second number by performing N0-N right shifts
                byte inputeByteToHide = byte.Parse(inputNumberConcealed.Text);
                inputeByteToHide = (byte)(inputeByteToHide >> (8 - bitsOfOutput));

                //Create the final byte by ORing the compressed concealed byte and the normalized concealing byte
                //we could also simply add the two, as there should be no overlap anymore.
                byte outputByte = (byte)(inputByte | inputeByteToHide);

                conceleaingNumberOut.Text = inputByte.ToString();
                concealedNumberOut.Text = inputeByteToHide.ToString();
                outputBox.Text = outputByte.ToString();

                //get what the recovered concealed number should look like 
                byte decodingCoefficient = (byte)(~encodingCoefficient);
                byte compressedOutput = (byte)(outputByte & decodingCoefficient);
                byte recoveredConcealedByte = (byte)(compressedOutput << (8 - bitsOfOutput));

                concealedNumberRecoveredOut.Text = recoveredConcealedByte.ToString();


            }
            catch
            {

            }


        }
    }
}