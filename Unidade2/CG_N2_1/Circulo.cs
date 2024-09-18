using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int NumeroPontos = 72;  // Número de pontos para desenhar o círculo
        private const double Raio = 0.5;  // Raio do círculo

        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineLoop;  // Usaremos LineLoop para desenhar o círculo como linhas conectadas

            // Adiciona os pontos do círculo a partir do centro
            AdicionarPontosCirculo(centro);
            Atualizar();
        }

        // Função para adicionar os pontos do círculo, distribuídos simetricamente no perímetro
        private void AdicionarPontosCirculo(Ponto4D centro)
        {
            double anguloIncremento = 2 * Math.PI / NumeroPontos;  // Incremento do ângulo para distribuir os pontos
            for (int i = 0; i < NumeroPontos; i++)
            {
                double angulo = i * anguloIncremento;
                double x = centro.X + Raio * Math.Cos(angulo);  // Calcula a coordenada X com base no centro e raio
                double y = centro.Y + Raio * Math.Sin(angulo);  // Calcula a coordenada Y com base no centro e raio
                base.PontosAdicionar(new Ponto4D(x, y));  // Adiciona os pontos calculados
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
