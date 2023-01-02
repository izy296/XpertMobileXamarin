// created on 25/03/2004 at 14:14
using System;
using System.Diagnostics;
using System.Text;
//namespace toto;

namespace Xpert.Common
{
    public class ChiffresLettres
    {
        public static string MoneyToLetter(decimal mont)
        {
            int entier = (int)mont / 1;
            int dec = (int)((mont - entier) * 100);
            string wordsEntiers = Xpert.Common.ChiffresLettres.Int2Lettres(entier);
            if (wordsEntiers.EndsWith("cent")) wordsEntiers += "s";
            string wordDecimal = Xpert.Common.ChiffresLettres.Int2Lettres(dec);
            if (wordDecimal.EndsWith("cent")) wordDecimal += "s";
            wordsEntiers += " dinars algériens et " + wordDecimal + " cts";
            return wordsEntiers;
        }
        
        /// <summary> /// Convertion d'un montant en toutes lettres /// </summary> /// <param name="values"></param> /// <returns></returns>
        private static string Int2Lettres(Int32 value)
        {
            //en cas de besoin pour vérifier l'orthographe
            //http://orthonet.sdv.fr/pages/lex_nombres.html 
            Int32 division, reste; StringBuilder sb;
            try
            {
                //Test l'état null 
                if (value == 0) return "zéro";
                //Décomposition de la valeur en milliards, millions, milliers,... 
                sb = new StringBuilder();
                //milliard
                division = Math.DivRem(value, 1000000000, out reste); if (division > 0)
                {
                    Int2LettresBloc(sb, division); sb.Append(" milliard"); if (division > 1) sb.Append('s');
                }
                if (reste > 0)
                {
                    //million 
                    value = reste; division = Math.DivRem(value, 1000000, out reste); if (division > 0)
                    {
                        if (sb.Length > 0) sb.Append(' '); Int2LettresBloc(sb, division); sb.Append(" million");
                        if (division > 1) sb.Append('s');
                    } if (reste > 0)
                    {
                        //milliers 
                        value = reste; division = Math.DivRem(value, 1000, out reste);
                        if (division > 0)
                        {
                            if (sb.Length > 0) sb.Append(' '); if (division == 1) sb.Append("mille");
                            else { Int2LettresBloc(sb, division); sb.Append(" mille"); }
                        } if (reste > 0)
                        {
                            //reste
                            if (sb.Length > 0) sb.Append(' '); Int2LettresBloc(sb, reste);
                        }
                    }
                } return sb.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message); return string.Empty;
            }
            finally { sb = null; }
        }
        /// <summary> /// Retourne la conversion d'un bloc de 3 bloc /// </summary> /// <param name="Value"></param> /// <returns></returns>
        private static void Int2LettresBloc(StringBuilder sb, Int32 value)
        {
            Boolean b_centaines; Int32 division, reste; try
            {
                division = Math.DivRem(value, 100, out reste);
                //Test si des centaines sont présentes 
                if (division > 0)
                { //ajout des centaines à la sortie 
                    switch (division)
                    { case 1: { sb.Append("cent"); break; } default: { Int2LettresBase(sb, division); sb.Append(" cent"); break; } }
                    b_centaines = true;
                }
                else { b_centaines = false; }
                //Test si il reste des éléments apres les centaines 
                if (reste > 0)
                {
                    //Introduction d'un espace si on a intégré des centaines 
                    if (b_centaines) sb.Append(' ');
                    //Calcul des dixaines et de leurs reste
                    value = reste; division = Math.DivRem(value, 10, out reste);
                    switch (division)
                    {
                        case 0:
                        case 1:
                        case 7:
                        case 9: { Int2LettresBase(sb, value); break; }
                        default:
                            {
                                Int2LettresBase(sb, division * 10); if (reste > 0)
                                {
                                    if (reste == 1)
                                        sb.Append(" et un");
                                    else { sb.Append('-'); Int2LettresBase(sb, reste); }
                                } break;
                            }
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        private static void Int2LettresBase(StringBuilder sb, Int32 value)
        {
            switch (value)
            {
                case 0: { sb.Append("zéro"); break; }
                case 1: { sb.Append("un"); break; }
                case 2: { sb.Append("deux"); break; }
                case 3: { sb.Append("trois"); break; }
                case 4: { sb.Append("quatre"); break; }
                case 5: { sb.Append("cinq"); break; }
                case 6: { sb.Append("six"); break; }
                case 7: { sb.Append("sept"); break; }
                case 8: { sb.Append("huit"); break; }
                case 9: { sb.Append("neuf"); break; }
                case 10: { sb.Append("dix"); break; }
                case 11: { sb.Append("onze"); break; }
                case 12: { sb.Append("douze"); break; }
                case 13: { sb.Append("treize"); break; }
                case 14: { sb.Append("quatorze"); break; }
                case 15: { sb.Append("quinze"); break; }
                case 16: { sb.Append("seize"); break; }
                case 17: { sb.Append("dix-sept"); break; }
                case 18: { sb.Append("dix-huit"); break; }
                case 19: { sb.Append("dix-neuf"); break; }
                case 20: { sb.Append("vingt"); break; }
                case 30: { sb.Append("trente"); break; }
                case 40: { sb.Append("quarante"); break; }
                case 50: { sb.Append("cinquante"); break; }
                case 60: { sb.Append("soixante"); break; }
                case 70: { sb.Append("soixante-dix"); break; }
                case 71: { sb.Append("soixante et onze"); break; }
                case 72: { sb.Append("soixante-douze"); break; }
                case 73: { sb.Append("soixante-treize"); break; }
                case 74: { sb.Append("soixante-quatorze"); break; }
                case 75: { sb.Append("soixante-quinze"); break; }
                case 76: { sb.Append("soixante-seize"); break; }
                case 77: { sb.Append("soixante-dix-sept"); break; }
                case 78: { sb.Append("soixante-dix-huit"); break; }
                case 79: { sb.Append("soixante-dix-neuf"); break; }
                case 80: { sb.Append("quatre-vingt"); break; }
                case 90: { sb.Append("quatre-vingt-dix"); break; }
                case 91: { sb.Append("quatre-vingt-onze"); break; }
                case 92: { sb.Append("quatre-vingt-douze"); break; }
                case 93: { sb.Append("quatre-vingt-treize"); break; }
                case 94: { sb.Append("quatre-vingt-quatorze"); break; }
                case 95: { sb.Append("quatre-vingt-quinze"); break; }
                case 96: { sb.Append("quatre-vingt-seize"); break; }
                case 97: { sb.Append("quatre-vingt-dix-sept"); break; }
                case 98: { sb.Append("quatre-vingt-dix-huit"); break; }
                case 99: { sb.Append("quatre-vingt-dix-neuf"); break; }
                case 100: { sb.Append("cent"); break; }
                default: { /*RAS*/ break; }
            }
        }
        public static string Converti(double chiffre)
        {
            int centaine, dizaine, unite, reste, y;
            bool dix = false;
            string lettre = "";
            //strcpy(lettre, "");

            reste = (int)chiffre / 1;

            for (int i = 1000000000; i >= 1; i /= 1000)
            {
                y = reste / i;
                if (y != 0)
                {
                    centaine = y / 100;
                    dizaine = (y - centaine * 100) / 10;
                    unite = y - (centaine * 100) - (dizaine * 10);
                    switch (centaine)
                    {
                        case 0:
                            break;
                        case 1:
                            lettre += "cent ";
                            break;
                        case 2:
                            if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                            else lettre += "deux cent ";
                            break;
                        case 3:
                            if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                            else lettre += "trois cent ";
                            break;
                        case 4:
                            if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                            else lettre += "quatre cent ";
                            break;
                        case 5:
                            if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                            else lettre += "cinq cent ";
                            break;
                        case 6:
                            if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                            else lettre += "six cent ";
                            break;
                        case 7:
                            if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                            else lettre += "sept cent ";
                            break;
                        case 8:
                            if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                            else lettre += "huit cent ";
                            break;
                        case 9:
                            if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                            else lettre += "neuf cent ";
                            break;
                    }// endSwitch(centaine)

                    switch (dizaine)
                    {
                        case 0:
                            break;
                        case 1:
                            dix = true;
                            break;
                        case 2:
                            lettre += "vingt ";
                            break;
                        case 3:
                            lettre += "trente ";
                            break;
                        case 4:
                            lettre += "quarante ";
                            break;
                        case 5:
                            lettre += "cinquante ";
                            break;
                        case 6:
                            lettre += "soixante ";
                            break;
                        case 7:
                            dix = true;
                            lettre += "soixante ";
                            break;
                        case 8:
                            lettre += "quatre-vingt ";
                            break;
                        case 9:
                            dix = true;
                            lettre += "quatre-vingt ";
                            break;
                    } // endSwitch(dizaine)

                    switch (unite)
                    {
                        case 0:
                            if (dix) lettre += "dix ";
                            break;
                        case 1:
                            if (dix) lettre += "onze ";
                            else lettre += "un ";
                            break;
                        case 2:
                            if (dix) lettre += "douze ";
                            else lettre += "deux ";
                            break;
                        case 3:
                            if (dix) lettre += "treize ";
                            else lettre += "trois ";
                            break;
                        case 4:
                            if (dix) lettre += "quatorze ";
                            else lettre += "quatre ";
                            break;
                        case 5:
                            if (dix) lettre += "quinze ";
                            else lettre += "cinq ";
                            break;
                        case 6:
                            if (dix) lettre += "seize ";
                            else lettre += "six ";
                            break;
                        case 7:
                            if (dix) lettre += "dix-sept ";
                            else lettre += "sept ";
                            break;
                        case 8:
                            if (dix) lettre += "dix-huit ";
                            else lettre += "huit ";
                            break;
                        case 9:
                            if (dix) lettre += "dix-neuf ";
                            else lettre += "neuf ";
                            break;
                    } // endSwitch(unite)

                    switch (i)
                    {
                        case 1000000000:
                            if (y > 1) lettre += "milliards ";
                            else lettre += "milliard ";
                            break;
                        case 1000000:
                            if (y > 1) lettre += "millions ";
                            else lettre += "million ";
                            break;
                        case 1000:
                            lettre += "mille ";
                            break;
                    }
                } // end if(y!=0)
                reste -= y * i;
                dix = false;
            } // end for
            if (lettre.Length == 0) lettre += "zero";

            return lettre;
        }
    }
}
