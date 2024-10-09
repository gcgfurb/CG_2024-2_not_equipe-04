using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int NumeroPontos = 72;  
        private const double Raio = 0.5;  

        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineLoop;

            AdicionarPontosCirculo(centro);
            Atualizar();
        }


        private void AdicionarPontosCirculo(Ponto4D centro)
        {
            double anguloIncremento = 2 * Math.PI / NumeroPontos; 
            for (int i = 0; i < NumeroPontos; i++)
            {
                double angulo = i * anguloIncremento;
                double x = centro.X + Raio * Math.Cos(angulo);  
                double y = centro.Y + Raio * Math.Sin(angulo);  
                base.PontosAdicionar(new Ponto4D(x, y));  
            }
        }

        // Atualiza o objeto no SRU
        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
